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
    internal partial class PluginConfigurationForm : Form
    {
        private IPlugin plugin;
        private SettingInfo[] settings;
        internal PluginConfigurationForm(PluginInfo pluginInfo)
        {
            InitializeComponent();
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel; // Default to cancel.
            this.plugin = pluginInfo.Instance;
            this.settings = pluginInfo.Settings;

            this.Text = String.Format("Plugin Configuration: {0}", pluginInfo.Name);
            //this.dataGridView1.DataError
        }

        private void PluginConfigurationForm_Load(object sender, EventArgs e)
        {
            this.LoadSettings();
        }

        private void LoadSettings()
        {
            this.dataGridView1.Rows.Clear();
            foreach (var setting in this.settings)
            {
                var row = new DataGridViewRow();
                row.Tag = setting;
                // Add setting name
                row.Cells.Add(new DataGridViewTextBoxCell { Value = setting.PropertyInfo.Name, ValueType = typeof(string) });

                // Add the setting value.
                var cell = this.CreateCell(setting);
                
                row.Cells.Add(cell);
                this.dataGridView1.Rows.Add(row);
                row.Cells[0].ReadOnly = true;
            }

            var heightModifier = 0;
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                heightModifier += row.Height - 1;
            }
            this.Height += Math.Min(640, heightModifier) -2;
        }

        private DataGridViewCell CreateCell(SettingInfo setting)
        {
            var dType = setting.PropertyInfo.PropertyType;
            if (dType.IsEnum)
            {
                var cell = new DataGridViewComboBoxCell
                {
                    DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                    Value = setting.PropertyInfo.GetValue(this.plugin, null),
                    ValueType = dType
                };

                foreach (var val in Enum.GetValues(dType))
                {
                    cell.Items.Add(val);
                }

                return cell;
            }
            else if (dType == typeof(bool))
            {
                var cell = new DataGridViewCheckBoxCell
                {
                    FalseValue = false,
                    TrueValue = true,
                    Value = setting.PropertyInfo.GetValue(this.plugin, null),
                    ValueType = dType
                };
                return cell;
            }
            // Default: Text field.
            else if (setting.SettingOptions != null && setting.SettingOptions.Length != 0)
            {
                var cell = new DataGridViewComboBoxCell
                {
                    DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox,
                    Value = setting.PropertyInfo.GetValue(this.plugin, null),
                    ValueType = dType
                };

                foreach (var option in setting.SettingOptions)
                {
                    cell.Items.Add(option.Value);
                }

                return cell;
            }
            else
            {
                var cell = new DataGridViewTextBoxCell
                {
                    Value = setting.PropertyInfo.GetValue(this.plugin, null),
                    ValueType = dType
                };
                return cell;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void PluginConfigurationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != System.Windows.Forms.DialogResult.OK) { return; }

            // Apply settings to instance.
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                var pi = row.Tag as SettingInfo;
                pi.PropertyInfo.SetValue(this.plugin, row.Cells[1].Value, null);
            }

            // Save settings.
            API.PluginConfig.SaveConfig(this.plugin);
        }
    }
}
