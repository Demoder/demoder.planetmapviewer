/*
* Demoder.PlanetMapViewer
* Copyright (C) 2012 Demoder (demoder@demoder.me)
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using Demoder.PlanetMapViewer.Xna;
using System.ComponentModel;
using Demoder.PlanetMapViewer.Helpers;

namespace Demoder.PlanetMapViewer.PmvApi
{
    public class PluginManager
    {
        public event Action PluginStateChangeEvent;

        private ConcurrentDictionary<Type, PluginInfo> registeredPlugins = new ConcurrentDictionary<Type, PluginInfo>();

        internal PluginInfo[] AllPlugins { get { return this.registeredPlugins.Values.ToArray(); } }

        internal void RegisterPlugins(Assembly assembly)
        {
            foreach (var plugin in assembly.GetTypes().Where(t => typeof(IPlugin).IsAssignableFrom(t) && t != typeof(IPlugin)))
            {
                this.RegisterPlugin(plugin);
            }
        }

        private void SendPluginStateChangeEvent()
        {
            Task.Factory.StartNew(this.RealSendPluginStateChangeEvent);
        }

        private void RealSendPluginStateChangeEvent()
        {
            var e = this.PluginStateChangeEvent;
            if (e == null) { return; }
            lock (e)
            {
                e();
            }
        }

        private void RegisterPlugin(Type type)
        {
            var pi = new PluginInfo
            {
                Type = type,
                Visible = true
            };

            var attr = type.GetCustomAttributes(typeof(PluginAttribute), true).FirstOrDefault() as PluginAttribute;
            var descAttr = type.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault() as DescriptionAttribute;
            if (attr == null)
            {
                pi.RefreshInterval = -1;
                pi.Name = pi.Type.Name;
            }
            else
            {
                pi.RefreshInterval = attr.RefreshInterval;
                pi.Name = attr.Name;
            }

            if (descAttr != null)
            {
                pi.Description = descAttr.Description;
            }

            pi.Settings = SettingInfo.Generate(pi.Type).OrderBy(s => s.PropertyInfo.Name).ToArray();
            this.registeredPlugins.TryAdd(type, pi);
        }

        internal bool LoadPlugin(Type type, bool sendEvent=true)
        {
            PluginInfo pi;
            if (!this.registeredPlugins.TryGetValue(type, out pi))
            {
                return false;
            }
            if (pi.Instance != null)
            {
                return false;
            }
            pi.Instance = Activator.CreateInstance(pi.Type) as IPlugin;
            API.PluginConfig.LoadConfig(pi.Instance);
            this.StartGenerationTask(pi);
            if (sendEvent) { this.SendPluginStateChangeEvent(); }
            return pi.Instance != null;
        }

        internal bool LoadPlugin<T>()
            where T : IPlugin
        {
            return this.LoadPlugin(typeof(T));
        }

        internal void UnloadPlugin<T>()
        {
            this.UnloadPlugin(typeof(T));
        }

        internal void LoadEnabledPlugins()
        {
            var pluginTypes = Properties.GeneralSettings.Default.EnabledPlugins.Split(new string[] { ";;" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var p in this.AllPlugins)
            {
                if (pluginTypes.Contains(p.Type.FullName))
                {
                    p.AutoLoad = true;
                    this.LoadPlugin(p.Type, false);
                }
            }
            this.SendPluginStateChangeEvent();
        }

        internal void UnloadPlugin(Type plugin)
        {
            PluginInfo pi;
            if (!this.registeredPlugins.TryGetValue(plugin, out pi))
            {
                return;
            }
            if (pi.Instance == null)
            {
                return;
            }

            var instance = pi.Instance;
            pi.Instance = null;
            instance.Dispose();
            this.SendPluginStateChangeEvent();
        }

        public T GetPlugin<T>()
            where T : IPlugin
        {
            PluginInfo pi;
            if (!this.registeredPlugins.TryGetValue(typeof(T), out pi))
            {
                return default(T);
            }
            return (T)pi.Instance;
        }

        internal void ChangePluginStatus<T>(bool enabled)
        {
            this.ChangePluginStatus(typeof(T), enabled);
        }

        internal void ChangePluginStatus(Type type, bool enabled)
        {
            PluginInfo pi;
            if (!this.registeredPlugins.TryGetValue(type, out pi))
            {
                return;
            }
            pi.Visible = enabled;
        }
        
        internal CustomMapOverlay[] GetMapOverlays()
        {
            var overlays = new List<CustomMapOverlay>();
            var sw = new Stopwatch();
            foreach (var p in this.registeredPlugins.Values.Where(pi => pi.Instance != null && pi.Visible).ToArray())
            {
                this.StartGenerationTask(p);
                overlays.Add(p.GeneratedOverlay);
            }
            return overlays.ToArray();
        }


        internal void SignalGenerationMre(Type type)
        {
            var pi = this.AllPlugins.FirstOrDefault(p => p.Type == type);
            if (pi == null) { return; }
            pi.GenerationMre.Set();
        }

        private void StartGenerationTask(PluginInfo pi)
        {
            if (pi.GenerationTask == null || pi.GenerationTask.IsCompleted)
            {
                pi.GenerationTask = Task.Factory.StartNew((Action<object>)this.GenerateMapOverlay, pi, TaskCreationOptions.LongRunning);
            }
        }

        #region Task methods
        private void GenerateMapOverlay(object obj)
        {
            if (obj == null || !(obj is PluginInfo)) { return; }
            var info = obj as PluginInfo;
            while (info.Instance != null)
            {
                var sw = Stopwatch.StartNew();
                try
                {
                    info.GeneratedOverlay = info.Instance.GetCustomOverlay();
                }
                catch (Exception ex)
                {
                    Program.WriteLog(ex.ToString());
                }
                sw.Stop();
                info.LastExecutionTime = sw.ElapsedMilliseconds;

                // Calculate pause delay.
                long delay = info.RefreshInterval;
                if (delay == -1)
                {
                    delay = (long)(1000 / TileDisplay.FrameFrequency);
                }
                delay -= sw.ElapsedMilliseconds;

                if (delay > 0)
                {
                    // If we haven't overshot interval, sleep.
                    info.GenerationMre.WaitOne((int)delay);
                    info.GenerationMre.Reset();
                }
            }
        }
        #endregion
    }
}
