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
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Demoder.PlanetMapViewer.Xna;
using Demoder.PlanetMapViewer.DataClasses;

namespace Demoder.PlanetMapViewer.Forms
{
    public partial class OptionWindow : Form
    {
        private Properties.MapSettings mapSettings { get { return Properties.MapSettings.Default; } }
        private Properties.WindowSettings windowSettings { get { return Properties.WindowSettings.Default; } }
        private Properties.GeneralSettings generalSettings { get { return Properties.GeneralSettings.Default; } }

        private Context context;

        public OptionWindow(Context context)
        {
            this.context = context;
            InitializeComponent();
            // Load settings
            this.AoPath.Text = this.mapSettings.AoPath;
            this.FPS.Value = Math.Min(this.generalSettings.FramesPerSecond, this.FPS.Maximum);
            this.overlayModeShowScrollbars.Checked = this.windowSettings.OverlaymodeShowScrollbars;
            this.overlayModeShowExitButton.Checked = this.windowSettings.OverlaymodeShowControlbox;
            this.overlayModeWorkaroundTopmost.Checked = this.windowSettings.OverlaymodeTopmostWorkaround;
            this.disableTutorials.Checked = this.generalSettings.DisableTutorials;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog1.SelectedPath = this.AoPath.Text;
            this.folderBrowserDialog1.ShowNewFolderButton = false;

            do
            {
                var dr = this.folderBrowserDialog1.ShowDialog();
                if (dr != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }

                if (IsValidAoFolder(this.folderBrowserDialog1.SelectedPath))
                {
                    this.AoPath.Text = this.folderBrowserDialog1.SelectedPath;
                    break;
                }
                this.MsgBoxInvalidAoFolder();
            } while (true);
        }

        public static bool IsValidAoFolder(string folder)
        {
            if (!File.Exists(Path.Combine(folder, "Anarchy.exe"))) { return false; }
            return true;
        }

        private void OkButtonClick(object sender, EventArgs e)
        {
            if (!IsValidAoFolder(this.AoPath.Text))
            {
                this.MsgBoxInvalidAoFolder();
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            // Save settings
            this.mapSettings.AoPath = this.AoPath.Text;

            this.generalSettings.DisableTutorials = this.disableTutorials.Checked;
            this.generalSettings.FramesPerSecond = (int)this.FPS.Value;
            TileDisplay.FrameFrequency = this.generalSettings.FramesPerSecond;

            this.windowSettings.OverlaymodeShowScrollbars = this.overlayModeShowScrollbars.Checked;
            this.windowSettings.OverlaymodeShowControlbox = this.overlayModeShowExitButton.Checked;
            this.windowSettings.OverlaymodeTopmostWorkaround = this.overlayModeWorkaroundTopmost.Checked;

            if (this.context.UiElements.ParentForm.OverlayModeToolStripMenuItem.Checked)
            {
                this.context.UiElements.ParentForm.ToggleOverlayMode();
            }

            this.mapSettings.Save();
            this.windowSettings.Save();
            this.generalSettings.Save();
            this.Close();
        }

        private void CancelButtonClick(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void MsgBoxInvalidAoFolder()
        {
            MessageBox.Show("This does not seem like a valid Anarchy Online installation folder!", "Invalid AO folder?", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
