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

namespace Demoder.PlanetMapViewer.PmvApi
{
    internal class PluginManager
    {
        private ConcurrentDictionary<Type, PluginInfo> registeredPlugins = new ConcurrentDictionary<Type, PluginInfo>();
        
        public void RegisterPlugins(Assembly assembly)
        {
            foreach (var plugin in assembly.GetTypes().Where(t => typeof(IPlugin).IsAssignableFrom(t) && t != typeof(IPlugin)))
            {
                this.RegisterPlugin(plugin);
            }
        }

        private void RegisterPlugin(Type type)
        {
            this.registeredPlugins.TryAdd(type, new PluginInfo { Type = type, Enabled = false });
        }

        public bool LoadPlugin(Type type)
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
            return pi.Instance != null;
        }

        public bool LoadPlugin<T>()
            where T : IPlugin
        {
            return this.LoadPlugin(typeof(T));
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

        public CustomMapOverlay[] GetMapOverlays()
        {
            var overlays = new List<CustomMapOverlay>();
            foreach (var p in this.registeredPlugins.Values.Where(pi => pi.Instance != null))
            {
                overlays.Add(p.Instance.GetCustomOverlay());
            }
            return overlays.ToArray();
        }


    }
}
