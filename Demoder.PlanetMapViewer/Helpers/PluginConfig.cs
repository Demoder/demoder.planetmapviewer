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
using Demoder.PlanetMapViewer.PmvApi;
using System.Collections.Concurrent;
using System.IO;
using Demoder.Common.Hash;
using System.Reflection;
using Demoder.Common.Attributes;
using Demoder.PlanetMapViewer.Plugins;
using Demoder.Common.Serialization;

namespace Demoder.PlanetMapViewer.Helpers
{
    public class PluginConfig
    {
        private DirectoryInfo storageDirectory;

        public PluginConfig(DirectoryInfo storageDirectory)
        {
            this.storageDirectory = storageDirectory;
            if (!this.storageDirectory.Exists) { this.storageDirectory.Create(); }
        }

        private FileInfo GetConfigFile(Type plugin)
        {
            var fileName = String.Format("{0}-{1}.xml",
                plugin.Name, 
                SHA1Checksum.Generate(plugin.GetType().FullName));

            var path = Path.Combine(
                this.storageDirectory.FullName,
                fileName);

            return new FileInfo(path);
        }

        public void LoadConfig(IPlugin plugin)
        {
            var conf = this.GetConfigFile(plugin.GetType());
            if (!conf.Exists)
            {
                this.GenerateDefaultConfig(plugin.GetType());
            }

            var settings = SettingInfo.Generate(plugin.GetType());
            var storedSettings = Xml.Deserialize<List<PluginSetting>>(conf, false);
            if (storedSettings == null)
            {
                this.GenerateDefaultConfig(plugin.GetType());
                storedSettings = Xml.Deserialize<List<PluginSetting>>(conf, false);
            }

            foreach (var setting in settings)
            {
                try 
                {
                    var storedValue = storedSettings.FirstOrDefault(s=>s.Name.Equals(setting.PropertyInfo.Name, StringComparison.InvariantCulture)).Value;
                    var value = Demoder.Common.TypeConverter.Convert(setting.PropertyInfo.PropertyType, storedValue);

                    setting.PropertyInfo.SetValue(
                        plugin,
                        value, 
                        null);
                }
                catch (Exception ex)
                {
                    Program.WriteLog(ex);
                }
            }

            API.PluginManager.SignalGenerationMre(plugin.GetType());
        }

        public void SaveConfig(IPlugin plugin)
        {
            var conf = this.GetConfigFile(plugin.GetType());
            var settings = SettingInfo.Generate(plugin.GetType());
            var storedSettings = new List<PluginSetting>();
            foreach (var setting in settings)
            {
                storedSettings.Add(new PluginSetting
                {
                    Name = setting.PropertyInfo.Name,
                    Value = setting.PropertyInfo.GetValue(plugin, null).ToString()
                });
            }
            Xml.Serialize(conf, storedSettings, false);
            API.PluginManager.SignalGenerationMre(plugin.GetType());
        }


        private void GenerateDefaultConfig(Type plugin)
        {
            var conf = this.GetConfigFile(plugin);
            var settings = SettingInfo.Generate(plugin);
            var storedSettings = new List<PluginSetting>();
            foreach (var setting in settings)
            {
                storedSettings.Add(new PluginSetting
                {
                     Name = setting.PropertyInfo.Name,
                     Value = setting.SettingAttribute.DefaultValue.ToString()
                });
            }
            Xml.Serialize(conf, storedSettings, false);
        }
    }

    internal class SettingInfo
    {
        public PropertyInfo PropertyInfo { get; private set; }
        public SettingAttribute SettingAttribute { get; private set; }
        public SettingOptionAttribute[] SettingOptions { get; private set; }

        private SettingInfo(){}
        public static SettingInfo[] Generate(Type plugin)
        {
            var settings = new List<SettingInfo>();
            var properties = from p in plugin.GetProperties()
                             where p.IsDefined(typeof(SettingAttribute), true)
                             select p;
            foreach (var p in properties)
            {
                var setting = new SettingInfo();
                setting.PropertyInfo = p;
                setting.SettingAttribute = p.GetCustomAttributes(typeof(SettingAttribute), true).FirstOrDefault() as SettingAttribute;
                setting.SettingOptions = p.GetCustomAttributes(typeof(SettingOptionAttribute), true).ToArray() as SettingOptionAttribute[];

                settings.Add(setting);
            }

            return settings.ToArray();
        }
    }
}
