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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Demoder.PlanetMapViewer.PmvApi;

namespace Demoder.PlanetMapViewer.Forms
{
    public partial class PluginListControl : UserControl
    {
        public PluginListControl()
        {
            InitializeComponent();
        }

        private void PluginListControl_Load(object sender, EventArgs e)
        {
            API.PluginManager.PluginStateChangeEvent += new PmvEvent<PlanetMapViewer.Events.PluginStateChangeEventArgs>(PluginManagerPluginStateChangeEvent);
            this.PopulatePluginList();
        }

        void PluginManagerPluginStateChangeEvent(object sender, Events.PluginStateChangeEventArgs e)
        {
            this.PopulatePluginList();
        }

        private void PopulatePluginList()
        {
            if (this.pluginList.InvokeRequired)
            {
                this.pluginList.Invoke((Action)this.PopulatePluginList);
                return;
            }

            lock (this.pluginList)
            {
                this.pluginList.BeginUpdate();
                this.pluginList.ItemChecked -= this.pluginListItemChecked;
                this.pluginList.Items.Clear();
                foreach (var plugin in API.PluginManager.AllPlugins)
                {
                    var lvi = new ListViewItem(plugin.Name);
                    lvi.Checked = plugin.Instance != null;
                    lvi.Tag = plugin;
                    pluginList.Items.Add(lvi);
                }

                this.pluginList.EndUpdate();
                this.pluginList.ItemChecked += this.pluginListItemChecked;
                return;
            }
        }

        private void pluginListItemChecked(object sender, ItemCheckedEventArgs e)
        {
            var pluginType = (e.Item.Tag as PluginInfo).Type;
            if (e.Item.Checked)
            {
                API.PluginManager.LoadPlugin(pluginType);
            }
            else
            {
                API.PluginManager.UnloadPlugin(pluginType);
            }
        }

        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.pluginList.SelectedItems.Count == 1)
            {
                var pi = ((PluginInfo)this.pluginList.SelectedItems[0].Tag);
                if (pi.Instance == null) { return; }
                if (pi.Settings.Length == 0)
                {
                    return;
                }
                var config = new PluginConfigurationForm(pi);
                config.ShowDialog();
            }
        }

        private void pluginContextStrip_Opening(object sender, CancelEventArgs e)
        {
            if (this.pluginList.SelectedItems.Count != 1)
            {
                e.Cancel = true;
                return;
            }
            if ((this.pluginList.SelectedItems[0].Tag as PluginInfo).Instance == null)
            {
                e.Cancel = true;
                return;
            }

            configureToolStripMenuItem.Enabled = (this.pluginList.SelectedItems[0].Tag as PluginInfo).Settings.Length > 0;
        }    
    }
}
