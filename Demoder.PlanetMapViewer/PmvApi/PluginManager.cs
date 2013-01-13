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
using Demoder.PlanetMapViewer.Events;
using System.IO;

namespace Demoder.PlanetMapViewer.PmvApi
{
    public class PluginManager
    {
        #region Events
        internal event PmvEvent<PluginStateChangeEventArgs> PluginStateChangeEvent;
        #endregion

        private BlockingCollection<EventArgs> events = new BlockingCollection<EventArgs>();

        public PluginManager()
        {
            Task.Factory.StartNew(this.SendEvents, TaskCreationOptions.LongRunning);
            // Load plugin assemblies
            DirectoryInfo pluginDir = new DirectoryInfo("Plugins");
            if (pluginDir.Exists)
            {
                foreach (FileInfo pluginFile in pluginDir.GetFiles("*.dll"))
                {
                    try
                    {
                        Assembly ass = Assembly.LoadFrom(pluginFile.FullName);
                        this.RegisterPlugins(ass);
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void SendEvents()
        {
            foreach (var e in this.events.GetConsumingEnumerable())
            {
                if (e.GetType() == typeof(PluginStateChangeEventArgs))
                {
                    this.SendEvent(this.PluginStateChangeEvent, e);
                }
            }
        }

        private void SendEvent(dynamic sendTo, dynamic e)
        {
            if (sendTo == null) { return; }
            lock (sendTo)
            {
                sendTo(this, e);
            }
        }


        private ConcurrentDictionary<Type, PluginInfo> registeredPlugins = new ConcurrentDictionary<Type, PluginInfo>();

        internal PluginInfo[] AllPlugins { get { return this.registeredPlugins.Values.ToArray(); } }

        internal void RegisterPlugins(Assembly assembly)
        {
            foreach (var plugin in assembly.GetTypes().Where(t => typeof(IPlugin).IsAssignableFrom(t) && t != typeof(IPlugin)))
            {
                this.RegisterPlugin(plugin);
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
            Program.WriteLog("Registering plugin: {0}", type);
        }

        internal bool LoadPlugin(Type type)
        {
            Program.WriteLog("Loading plugin {0}", type);
            PluginInfo pi;
            if (!this.registeredPlugins.TryGetValue(type, out pi))
            {
                Program.WriteLog("\tFailed to load plugin; Type isn't registered as plugin");
                return false;
            }
            if (pi.Instance != null)
            {
                Program.WriteLog("\tFailed to load plugin: Already loaded.");
                return false;
            }
            Program.WriteLog("\tCreating instance of plugin");
            pi.Instance = Activator.CreateInstance(pi.Type) as IPlugin;
            Program.WriteLog("\tLoading configuration for plugin");
            API.PluginConfig.LoadConfig(pi.Instance);
            Program.WriteLog("\tStarting generation task for plugin");
            this.StartGenerationTask(pi);

            Program.WriteLog("\tSending PluginStateChangeEvent because of loading plugin");
            this.events.Add(new PluginStateChangeEventArgs(pi, pi.Instance != null));
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
            Program.WriteLog("------------");
            var pluginTypes = Properties.GeneralSettings.Default.EnabledPlugins.Split(new string[] { ";;" }, StringSplitOptions.RemoveEmptyEntries);
            Program.WriteLog("Loading enabled plugins:\r\n\t{0}", String.Join("\r\n\t", pluginTypes));
            foreach (var p in this.AllPlugins)
            {
                if (pluginTypes.Contains(p.Type.FullName))
                {
                    Program.WriteLog("--- Plugin is configured to autoload: {0} ---", p.Type);
                    p.AutoLoad = true;
                    this.LoadPlugin(p.Type);
                }
            }
            Program.WriteLog("------------");
        }

        internal void UnloadPlugin(Type plugin)
        {
            Program.WriteLog("Unloading plugin {0}", plugin);
            PluginInfo pi;
            if (!this.registeredPlugins.TryGetValue(plugin, out pi))
            {
                Program.WriteLog("\tFailed to unload plugin because it's not a registered plugin: {0}", plugin);
                return;
            }
            if (pi.Instance == null)
            {
                Program.WriteLog("\tFailed to unload plugin because it's not loaded: {0}", plugin);
                return;
            }

            var instance = pi.Instance;
            pi.Instance = null;
            Program.WriteLog("\tDisposing plugin: {0}", plugin);
            instance.Dispose();
            Program.WriteLog("\tSending plugin state change event.");
            this.events.Add(new PluginStateChangeEventArgs(pi, pi.Instance != null));
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


        internal void ChangeLayerStatus<T>(string layerName, bool enabled)
        {
            this.ChangeLayerStatus(typeof(T), layerName, enabled);
        }

        internal void ChangeLayerStatus(Type type, string layerName, bool enabled)
        {
            PluginInfo pi;
            if (!this.registeredPlugins.TryGetValue(type, out pi))
            {
                return;
            }

            pi.HiddenOverlays.Remove(layerName);

            if (!enabled)
            {
                pi.HiddenOverlays.Add(layerName);
            }
        }
        
        internal MapOverlay[] GetMapOverlays()
        {
            var overlays = new List<MapOverlay>();
            var sw = new Stopwatch();
            foreach (var p in this.registeredPlugins.Values.Where(pi => pi.Instance != null && pi.Visible).ToArray())
            {
                this.StartGenerationTask(p);
                if (p.GeneratedOverlay == null || p.GeneratedOverlay.Count() == 0)
                {
                    continue;
                }

                foreach (var overlay in p.GeneratedOverlay)
                {
                    if (overlay == null || overlay.MapItems.Count == 0)
                    {
                        continue;
                    }
                    if (p.HiddenOverlays.Contains(overlay.Name)) { continue; }
                    overlays.Add(overlay);
                }
            }
            return overlays.OrderBy(o=>o.DrawOrder).ToArray();
        }

        internal void RedrawLayers(Type plugin)
        {
            Program.WriteLog("Signaling generation MRE for plugin {0}", plugin);
            var pi = this.AllPlugins.FirstOrDefault(p => p.Type == plugin);
            if (pi == null)
            {
                Program.WriteLog("\tFailed to signal generation MRE for plugin because plugin isn't registered: {0}", plugin);
                return;
            }
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
                    var overlays = new List<MapOverlay>();
                    foreach (var overlay in info.Instance.GetCustomOverlay()) 
                    {
                        overlays.Add(overlay);
                    }
                    info.GeneratedOverlay = overlays.ToArray();
                }
                catch (Exception ex)
                {
                    Program.WriteLog(ex);
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
