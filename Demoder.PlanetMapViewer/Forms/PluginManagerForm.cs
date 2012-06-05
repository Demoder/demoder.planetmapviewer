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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Demoder.PlanetMapViewer.PmvApi;
using Demoder.PlanetMapViewer.Helpers;

namespace Demoder.PlanetMapViewer.Forms
{
    public partial class PluginManagerForm : Form
    {
        public PluginManagerForm()
        {
            InitializeComponent();
            API.PluginManager.PluginStateChangeEvent += new Action(PluginManager_PluginStateChangeEvent);
        }

        void PluginManager_PluginStateChangeEvent()
        {
            this.UpdateList();
        }

        private void PluginManager_Load(object sender, EventArgs e)
        {
            this.UpdateList();
        }

        private void UpdateList()
        {
            if (this.listView1.InvokeRequired)
            {
                this.listView1.Invoke((Action)this.UpdateList);
                return;
            }

            this.listView1.BeginUpdate();
            this.listView1.ItemChecked -= this.listView1_ItemChecked;
            this.listView1.Items.Clear();
            foreach (var pi in API.PluginManager.AllPlugins)
            {
                var lvi = new ListViewItem();
                lvi.Text = pi.Name;
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, String.Format("{0:N0} ms",pi.LastExecutionTime)){ Name = "LastExecutionTime"});
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, pi.RefreshInterval == -1 ? "ASAP" : String.Format("{0:N0} ms",pi.RefreshInterval)){ Name = "RefreshInterval"});
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, pi.Description) { Name = "Description" });
                
                if (pi.AutoLoad)
                {
                    lvi.Group = this.listView1.Groups["lvGroupEnabled"];
                }
                else
                {
                    lvi.Group = this.listView1.Groups["lvGroupDisabled"];
                }

                lvi.Tag = pi;

                lvi.Checked = pi.Instance != null;
                this.listView1.Items.Add(lvi);
            }
            this.listView1.ItemChecked += this.listView1_ItemChecked;
            this.listView1.EndUpdate();
        }

        private void ContextOpening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip cms = sender as ContextMenuStrip;

            bool load = false;
            bool unload = false;

            cms.Items["refreshList"].Visible = this.listView1.SelectedItems.Count == 0;

            foreach (ListViewItem item in this.listView1.SelectedItems)
            {
                if (item.Group.Name == "lvGroupDisabled") 
                {
                    load = true;
                    continue;
                }
                if (item.Group.Name == "lvGroupEnabled") { unload = true; }
            }

            cms.Items["loadPlugin"].Visible = load;
            cms.Items["unloadPlugin"].Visible = unload;

            if (this.listView1.SelectedItems.Count == 1)
            {
                configureToolStripMenuItem.Visible = (((PluginInfo)this.listView1.SelectedItems[0].Tag).Instance != null);
            }
            else
            {
                configureToolStripMenuItem.Visible = false;
            }
        }

        #region Click events
        private void RefreshClick(object sender, EventArgs e)
        {
            this.UpdateList();
        }
        
        private void Button1Click(object sender, EventArgs e)
        {
            var types = from p in API.PluginManager.AllPlugins
                        where p.AutoLoad
                        select p.Type.FullName;

            Properties.GeneralSettings.Default.EnabledPlugins = String.Join(";;", types);
            Properties.GeneralSettings.Default.Save();
            this.Close();
        }
      

        private void loadPlugin_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.listView1.SelectedItems)
            {
                var pi = item.Tag as PluginInfo;
                pi.AutoLoad = true;
            }
            this.UpdateList();
        }

        private void unloadPlugin_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.listView1.SelectedItems)
            {
                var pi = item.Tag as PluginInfo;
                pi.AutoLoad = false;
            }
            this.UpdateList();
        }

        #endregion

        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 1)
            {
                var pi = ((PluginInfo)this.listView1.SelectedItems[0].Tag);
                if (pi.Instance == null) {return;}
                if (pi.Settings.Length == 0)
                {
                    return;
                }
                var config = new PluginConfigurationForm(pi);
                config.ShowDialog();
            }
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (e.Item.Checked)
            {
                API.PluginManager.LoadPlugin((e.Item.Tag as PluginInfo).Type);
            }
            else
            {
                API.PluginManager.UnloadPlugin((e.Item.Tag as PluginInfo).Type);
            }
        }
    }
}
