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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using sysDrawing = System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using thrd = System.Threading;
using System.Windows.Forms;
using Demoder.AoHook;
using Demoder.Common;
using Demoder.PlanetMapViewer.DataClasses;
using Demoder.PlanetMapViewer.Helpers;
using Demoder.PlanetMapViewer.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Demoder.PlanetMapViewer.PmvApi;
using Demoder.PlanetMapViewer.Plugins;
using System.Threading.Tasks;
using System.Threading;
using System.Configuration;

namespace Demoder.PlanetMapViewer.Forms
{
    public partial class MainWindow : Form
    {
        #region Members
        private Stopwatch lastException;
        private ApplicationSettingsBase[] availableSettings = new ApplicationSettingsBase[]
                {
                    Properties.GeneralSettings.Default,
                    Properties.MapSettings.Default,
                    Properties.WindowSettings.Default,
                    Properties.NormalTutorial.Default,
                    Properties.OverlayTutorial.Default,
                    Properties.GuiFonts.Default,
                    Properties.MapFonts.Default
                };

        #region Timers
        private thrd.Timer topMostTimer;
        #endregion

        private string screenShotFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Demoders PlanetMap Viewer");
        private BackgroundWorker bgwVersionCheck = new BackgroundWorker();
        #endregion

        #region Events
        public event MainFormModeChangeDelegate ModeChanged;
        #endregion

        #region Form setup
        public MainWindow()
        {
            try
            {
                Program.WriteLog("MainWindow->MainWindow()");
                InitializeComponent();
                this.splitContainer1.SplitterDistance = 535;
                Program.WriteLog("MainWindow->MainWindow(): InitializeComponent was successfull.");
            }
            catch (Exception ex)
            {
                Program.WriteLog(ex);
                this.ShowExceptionError(ex);
            }
        }

        private void MainWindowFormLoad(object sender, EventArgs e)
        {
            try
            {
                Program.WriteLog("MainWindow->Form1_Load() A");
                API.UiElements.HScrollBar = this.tileDisplay1_hScrollBar;
                API.UiElements.VScrollBar = this.tileDisplay1_vScrollBar;
                API.UiElements.ParentForm = this;

                Program.WriteLog("MainWindow->Form1_Load() B");
                // Check if we should attempt to upgrade settings
                Program.WriteLog("MainWindow->Form1_Load(): Checking settings");
                
                if (Properties.GeneralSettings.Default.SettingVersion != this.ProductVersion.ToString())
                {
                    Properties.GeneralSettings.Default.SettingVersion = this.ProductVersion.ToString();

                    foreach (var s in this.availableSettings)
                    {
                        s.Upgrade();
                        s.Save();
                    }
                }
                Program.WriteLog("MainWindow->Form1_Load() Settings OK");

                Program.WriteLog("MainWindow->Form1_Load() Checking AO folder");
                if (!OptionWindow.IsValidAoFolder(Properties.MapSettings.Default.AoPath))
                {
                    Program.WriteLog("MainWindow->Form1_Load(): AO folder is invalid. Showing options dialog to user.");
                    var dr = this.ShowOptionsDialog("Demoder's Planet Map Viewer - Options");
                    if (dr == DialogResult.Cancel)
                    {
                        Program.WriteLog("MainWindow->Form1_Load() User canceled AO folder selection.");
                        Application.Exit();
                        return;
                    }
                }

                Program.WriteLog("MainWindow->Form1_Load() Initializing SpriteBatch");
                API.SpriteBatch = new SpriteBatch(API.GraphicsDevice);
                Program.WriteLog("MainWindow->Form1_Load() Initializing MapManager");
                API.MapManager = new MapManager();
                Program.WriteLog("MainWindow->Form1_Load() Initializing Camera");
                API.Camera = new Camera();
                Program.WriteLog("MainWindow->Form1_Load() Initializing MapManager (stage 2)");
                API.MapManager.Initialize();

                // "Gracefully" handle situations where we have no valid maps.
                if (API.MapManager.CurrentMap == null)
                {
                    var dr = this.ShowOptionsDialog("Demoder's Planet Map Viewer - Options");
                    if (dr == DialogResult.Cancel)
                    {
                        Application.Exit();
                    }
                    API.MapManager.Initialize();
                    if (API.MapManager.CurrentMap == null)
                    {
                        Application.Exit();
                    }

                }

                Program.WriteLog("MainWindow->Form1_Load() Assigning events");
                this.bgwVersionCheck.DoWork += bgwVersionCheck_DoWork;
                this.bgwVersionCheck.RunWorkerCompleted += bgwVersionCheck_RunWorkerCompleted;
                Program.WriteLog("MainWindow->Form1_Load() Applying settings");
                this.ApplySettings();
                Program.WriteLog("MainWindow->Form1_Load() Adjusting scrollbars to layer");
                API.Camera.AdjustScrollbarsToLayer();

                // Setup the tile display.
                Program.WriteLog("MainWindow->Form1_Load() Assigning window handle to tileDisplay1.Handle");
                Mouse.WindowHandle = this.tileDisplay1.Handle;
            }
            catch (Exception ex)
            {
                Program.WriteLog(ex);
                this.ShowExceptionError(ex);
                Application.Exit();
            }

            try
            {
                API.AoHook = new HookInfoTracker();
                API.PluginManager.RegisterPlugins(this.GetType().Assembly);
                if (String.IsNullOrWhiteSpace(Properties.GeneralSettings.Default.EnabledPlugins))
                {
                    var plugins = from p in API.PluginManager.AllPlugins
                                  where p.Type == typeof(CharacterLocatorPlugin)
                                  select p.Type.FullName;
                    Properties.GeneralSettings.Default.EnabledPlugins =
                        String.Join(";;", plugins);
                }

                // Load any enabled plugins
                API.PluginManager.LoadEnabledPlugins();

                // Ask about tutorials
                if (!Properties.GeneralSettings.Default.HaveAskedForTutorials && TutorialPlugin.CurrentStage != TutorialStage.Completed)
                {
                    var ret = MessageBox.Show("Do you want to see the tutorial?", "Tutorial", MessageBoxButtons.YesNoCancel);
                    switch (ret)
                    {
                        case System.Windows.Forms.DialogResult.Yes:
                            Properties.GeneralSettings.Default.HaveAskedForTutorials = true;
                            Properties.GeneralSettings.Default.DisableTutorials = false;
                            API.PluginManager.LoadPlugin<TutorialPlugin>();                            
                            Properties.GeneralSettings.Default.Save();
                            break;
                        case System.Windows.Forms.DialogResult.No:
                            Properties.GeneralSettings.Default.HaveAskedForTutorials = true;
                            Properties.GeneralSettings.Default.DisableTutorials = true;
                            API.PluginManager.UnloadPlugin<TutorialPlugin>();
                            Properties.GeneralSettings.Default.Save();
                            break;
                    }
                }

                API.AoHook.Provider.HookAo();
            }
            catch (Exception ex)
            {
                Program.WriteLog(ex);
            }
        }

        private void ShowExceptionError(Exception ex)
        {
            if (this.lastException == null || this.lastException.ElapsedMilliseconds > 10000)
            {
                this.lastException = Stopwatch.StartNew();
                MessageBox.Show(ex.Message, "Unexpected exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainWindowFormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.OverlayModeToolStripMenuItem.Checked)
            {
                this.OverlayModeToolStripMenuItem.Checked = false;
                e.Cancel = true;
                this.ToggleOverlayMode();
                return;
            }
            // Store window states
            var settings = Properties.WindowSettings.Default;
            settings.WindowFullscreen = this.fullscreenToolStripMenuItem.Checked;
            if (!settings.WindowFullscreen)
            {
                settings.WindowState = this.WindowState;
                settings.WindowPosition = this.Location;
                if (settings.WindowState == FormWindowState.Normal)
                {
                    settings.WindowSize = new System.Drawing.Point(this.Width, this.Height);
                }
            }

            // Save setting files
            foreach (var setting in this.availableSettings)
            {
                setting.Save();
            }
        }
        #endregion

        #region Version checking
        private void bgwVersionCheck_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result == null)
            {
                MessageBox.Show("Unexpected server response, please try again later.", "Version information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var res = e.Result as VersionInfo;
            if (res.UpdateAvailable(new Version(this.ProductVersion)))
            {
                var dr = MessageBox.Show("New version " + res.ProductVersion.ToString() + " is available! Download now?", "Version information", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                // Todo: Actually download update!
                var uri = new Uri(res.DownloadUri);
                if (uri.IsFile)
                {
                    MessageBox.Show(String.Format(
                        "Ooops! Some error occured: {0}. Don't panic! But please contact the author of this application and let them know.",
                        ((int)(VersionInfo.ErrorCodes.InvalidUri | VersionInfo.ErrorCodes.Security))
                        ),
                        "Error",
                        MessageBoxButtons.OK);
                }
                System.Diagnostics.Process.Start(uri.ToString());
                return;
            }
            else
            {
                MessageBox.Show("There's no available updates.", "Version information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void bgwVersionCheck_DoWork(object sender, DoWorkEventArgs e)
        {
            var res = VersionInfo.GetInfo("PlanetMapViewer");
            e.Result = res;
        }
        #endregion


        #region Features specific to overlay mode
        protected override void WndProc(ref Message m)
        {
            if (this.OverlayModeToolStripMenuItem == null || !this.OverlayModeToolStripMenuItem.Checked)
            {
                base.WndProc(ref m);
                return;
            }

            if (m.Msg == Win32MessageCodes.WM_NCRBUTTONDOWN)
            {
                this.OverlayTitleContextMenuStrip.Show(Control.MousePosition);
                return;
            }

            base.WndProc(ref m);
        }
        #endregion

        /// <summary>
        /// Applies form-related settings.
        /// </summary>
        private void ApplySettings()
        {
            var windowSettings = Properties.WindowSettings.Default;
            var generalSettings = Properties.GeneralSettings.Default;
            this.Location = windowSettings.WindowPosition;

            if (windowSettings.WindowFullscreen)
            {
                this.fullscreenToolStripMenuItem.Checked = true;
                API.State.WindowMode = WindowMode.Fullscreen;
                this.ToggleFullscreenSetting();
            }
            else
            {
                this.WindowState = windowSettings.WindowState;
                API.State.WindowMode = WindowMode.Normal;
                if (this.WindowState == FormWindowState.Normal)
                {
                    this.Width = windowSettings.WindowSize.X;
                    this.Height = windowSettings.WindowSize.Y;
                }
            }


            // Make sure FPS is valid.
            var fps = generalSettings.FramesPerSecond;
#if DEBUG
            if (fps > 999)
#else
            if (fps>30)
#endif
            {
                generalSettings.FramesPerSecond = 30;
                generalSettings.Save();
            }
            else if (fps < 1)
            {
                generalSettings.FramesPerSecond = 1;
                generalSettings.Save();
            }
            TileDisplay.FrameFrequency = generalSettings.FramesPerSecond;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.W:
                case System.Windows.Forms.Keys.A:
                case System.Windows.Forms.Keys.S:
                case System.Windows.Forms.Keys.D:
                case System.Windows.Forms.Keys.Oemplus:
                case System.Windows.Forms.Keys.OemMinus:
                case System.Windows.Forms.Keys.Zoom:
                case System.Windows.Forms.Keys.Add:
                case System.Windows.Forms.Keys.Subtract:
                    this.tileDisplay1.HandleKeyDown(e);
                    return;
            }
            base.OnKeyDown(e);
        }
     

        

        FormWindowState oldState = FormWindowState.Normal;


        #region Misc features
        private void SaveScreenShot()
        {
            var bmpScreenshot = new sysDrawing.Bitmap(this.Width, this.Height, PixelFormat.Format32bppArgb);
            var gfxScreenshot = sysDrawing.Graphics.FromImage(bmpScreenshot);

            gfxScreenshot.CopyFromScreen(this.Left, this.Top, 0, 0, this.Size, sysDrawing.CopyPixelOperation.SourceCopy);


            gfxScreenshot.Save();
            gfxScreenshot.Dispose();

            int num = 0;
            
            var nameFormat = "Screenshot_{0}.png";
            if (!Directory.Exists(this.screenShotFolder))
            {
                Directory.CreateDirectory(this.screenShotFolder);
            }

            while (File.Exists(Path.Combine(this.screenShotFolder,String.Format(nameFormat, num))))
            {
                num++;
            }

            bmpScreenshot.Save(Path.Combine(this.screenShotFolder, String.Format(nameFormat, num)), ImageFormat.Png);
        }
        #endregion

        #region Toolstrip
        ////////////
        // TOOLS
        ////////////

        // Screenshot
        private void MenuToolsScreenshot(object sender, EventArgs e)
        {
            this.SaveScreenShot();
        }


        //////////////////
        // VIEW
        //////////////////

        // Fullscreen
        private void MenuViewFullscreen(object sender, EventArgs e)
        {
            if (fullscreenToolStripMenuItem.Checked)
            {
                API.State.WindowMode = WindowMode.Fullscreen;
            }
            else
            {
                API.State.WindowMode = WindowMode.Normal;
            }
            this.ToggleFullscreenSetting();
        }


        /////////////////////////////////////
        // Help
        ////////////////////////////////////

        // Update
        private void checkVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.bgwVersionCheck.IsBusy)
            {
                MessageBox.Show("Already checking version!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            this.bgwVersionCheck.RunWorkerAsync();
        }

        // About
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new AboutBox1();
            a.ShowDialog();
        }
        #endregion

        private void tileDisplay1_ScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            API.State.CameraControl = CameraControl.Manual;
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowOptionsDialog();
        }

        private DialogResult ShowOptionsDialog(string customTitle = null)
        {
            var options = new OptionWindow();
            options.StartPosition = FormStartPosition.CenterParent;
            if (customTitle != null)
            {
                options.Text = customTitle;
            }
            return options.ShowDialog();
        }

        private void readmeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("Readme.txt");
        }

        private void hideSidebarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ToggleOverlayMode();
        }

        internal void ToggleFullscreenSetting()
        {
            var oldMode = API.State.WindowMode;
            if (this.fullscreenToolStripMenuItem.Checked)
            {
                this.OverlayModeToolStripMenuItem.Checked = false;
                this.ToggleOverlayMode();

                this.oldState = this.WindowState;
                this.WindowState = FormWindowState.Maximized;
                this.ControlBox = false;
                this.statusStrip1.Visible = false;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.Padding = new Padding(2, 3, 5, 10);
                if (this.ModeChanged!=null)
                {
                    this.ModeChanged(oldMode, WindowMode.Fullscreen);
                }
            }

            else
            {
                this.WindowState = this.oldState;
                this.ControlBox = true;
                this.menuStrip1.Visible = true;
                this.statusStrip1.Visible = true;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                this.Padding = new Padding(0);
                if (this.ModeChanged != null)
                {
                    this.ModeChanged(oldMode, WindowMode.Normal);
                }
            }
        }

        internal void ToggleOverlayMode()
        {
            var oldMode = API.State.WindowMode;

            if (this.OverlayModeToolStripMenuItem.Checked)
            {
                this.TopMost = true;

                // Hack for cases where a topmost window still gets hidden behind other windows at random intervals.
                if (Properties.WindowSettings.Default.OverlaymodeTopmostWorkaround)
                {
                    if (this.topMostTimer != null) { this.topMostTimer.Dispose(); }
                    this.topMostTimer = new thrd.Timer((thrd.TimerCallback)delegate(object obj)
                    {
                        this.ForceTopMost();
                    }, null, 250, 250);
                }

                if (this.fullscreenToolStripMenuItem.Checked)
                {
                    this.fullscreenToolStripMenuItem.PerformClick();
                }
                this.menuStrip1.Visible = false;
                this.statusStrip1.Visible = false;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;

                this.MinimumSize = new sysDrawing.Size(100, 100);

                this.tileDisplay1_hScrollBar.Visible = Properties.WindowSettings.Default.OverlaymodeShowScrollbars;
                this.tileDisplay1_vScrollBar.Visible = Properties.WindowSettings.Default.OverlaymodeShowScrollbars;

                this.ControlBox = Properties.WindowSettings.Default.OverlaymodeShowControlbox;
                this.splitContainer1.Panel2Collapsed = true;
                API.State.WindowMode = WindowMode.Overlay;

                if (this.ModeChanged != null)
                {
                    this.ModeChanged(oldMode, WindowMode.Overlay);
                }
            }
            else
            {
                this.MinimumSize = new sysDrawing.Size(400, 500);
                if (this.Width < this.MinimumSize.Width) { this.Width = this.MinimumSize.Width; }
                if (this.Height < this.MinimumSize.Height) { this.Height = this.MinimumSize.Height; }

                if (this.topMostTimer != null)
                {
                    this.topMostTimer.Dispose();
                    this.topMostTimer = null;
                }
                this.TopMost = false;

                this.splitContainer1.Panel2Collapsed = false;
                this.menuStrip1.Visible = true;
                this.statusStrip1.Visible = true;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;

                this.tileDisplay1_hScrollBar.Visible = true;
                this.tileDisplay1_vScrollBar.Visible = true;

                this.ControlBox = true;
                API.State.WindowMode = WindowMode.Normal;

                if (this.ModeChanged != null)
                {
                    this.ModeChanged(oldMode, WindowMode.Normal);
                }
            }
        }

        private void ForceTopMost()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action)this.ForceTopMost);
                return;
            }

            this.TopMost = false;
            this.TopMost = true;
            this.SetTopLevel(true);
        }

        private void OverlayTitleContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            lock (this.OverlayTitleContextMenuStrip)
            {
                // Region
                this.rubikaToolStripMenuItem1.Checked = this.mapSelectionControl1.RadioRK.Checked;
                this.shadowlandsToolStripMenuItem1.Checked = this.mapSelectionControl1.RadioSL.Checked;
                this.autoToolStripMenuItem.Checked = this.mapSelectionControl1.RadioAuto.Checked;

                // Camera controls
                this.followCharactersToolStripMenuItem1.Checked = API.State.CameraControl == CameraControl.Character;

                #region Map selection
                {
                    // Dispose of the old items.
                    ToolStripMenuItem[] menuItems = new ToolStripMenuItem[selectMapToolStripMenuItem.DropDownItems.Count];
                    selectMapToolStripMenuItem.DropDownItems.CopyTo(menuItems, 0);
                    foreach (ToolStripMenuItem i in menuItems)
                    {
                        i.Click -= this.OverlayTitleContextMenu_MapSelectionItemClickEventHandler;
                        i.Dispose();
                    }
                    selectMapToolStripMenuItem.DropDownItems.Clear();
                    foreach (var i in this.mapSelectionControl1.MapComboBox.Items)
                    {
                        var map = i as MapSelectionItem;
                        var item = new ToolStripMenuItem(map.MapName);
                        item.Tag = map.MapPath;
                        item.Click += this.OverlayTitleContextMenu_MapSelectionItemClickEventHandler;
                        if (this.mapSelectionControl1.MapComboBox.SelectedItem == map)
                        {
                            item.Checked = true;
                            item.Enabled = false;
                        }
                        selectMapToolStripMenuItem.DropDownItems.Add(item);
                    }

                    this.selectMapToolStripMenuItem.Enabled = this.selectMapToolStripMenuItem.DropDownItems.Count != 0;
                }
                #endregion

                #region Follow characters selection
                {
                    // Dispose old items
                    ToolStripMenuItem[] menuItems = new ToolStripMenuItem[this.selectCharactersToolStripMenuItem.DropDownItems.Count];
                    this.selectCharactersToolStripMenuItem.DropDownItems.CopyTo(menuItems, 0);
                    foreach (ToolStripMenuItem i in menuItems)
                    {
                        i.Click -= this.OverlayTitleContextMenu_CharacterSelectionItemClickEventHandler;
                        i.Dispose();
                    }

                    this.selectCharactersToolStripMenuItem.DropDownItems.Clear();
                    var characters = API.State.PlayerInfo.Values.ToArray();
                    foreach (var character in characters)
                    {
                        try
                        {
                            if (String.IsNullOrWhiteSpace(character.Name)) { continue; }

                            var item = new ToolStripMenuItem(character.Name);
                            if (character.InShadowlands)
                            {
                                item.Image = Properties.CharacterTrackerControlResources.PlayerInShadowlands;
                            }
                            else
                            {
                                item.Image = Properties.CharacterTrackerControlResources.PlayerOnRubika;
                            }

                            if (!character.IsHooked)
                            {
                                item.ForeColor = sysDrawing.SystemColors.GrayText;
                            }

                            item.Tag = character.ID;
                            item.Checked = character.IsTrackedByCamera;
                            item.Click += this.OverlayTitleContextMenu_CharacterSelectionItemClickEventHandler;
                            this.selectCharactersToolStripMenuItem.DropDownItems.Add(item);
                        }
                        catch (Exception ex)
                        {
                            Program.WriteLog(ex);
                        }
                    }
                    this.selectCharactersToolStripMenuItem.Enabled = this.selectCharactersToolStripMenuItem.DropDownItems.Count != 0;
                }
                #endregion

                #region Plugin management
                {
                   var menuItems = new ToolStripMenuItem[pluginsToolStripMenuItem.DropDownItems.Count];
                    this.pluginsToolStripMenuItem.DropDownItems.CopyTo(menuItems, 0);
                    foreach (ToolStripMenuItem i in menuItems)
                    {
                        i.CheckedChanged -= lvi_CheckedChanged;
                        i.Dispose();
                    }

                    foreach (var plugin in API.PluginManager.AllPlugins.OrderBy(p=>p.Name))
                    {
                        var tsi = new ToolStripMenuItem(plugin.Name);
                        tsi.Checked = plugin.Instance != null;
                        tsi.Tag = plugin;
                        tsi.CheckedChanged += lvi_CheckedChanged;
                        tsi.CheckOnClick = true;
                        pluginsToolStripMenuItem.DropDownItems.Add(tsi);
                    }
                }
                #endregion
            }
        }

        private void lvi_CheckedChanged(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            var plugin = item.Tag as PluginInfo;
            if (item.Checked)
            {
                API.PluginManager.LoadPlugin(plugin.Type);
            }
            else 
            {
                API.PluginManager.UnloadPlugin(plugin.Type);
            }

            this.OverlayTitleContextMenuStrip.Show();
            this.pluginsToolStripMenuItem.ShowDropDown();
        }

        void OverlayTitleContextMenu_MapSelectionItemClickEventHandler(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                var item = sender as ToolStripMenuItem;
                API.MapManager.SelectMap(item.Tag.ToString());
            }
        }

        void OverlayTitleContextMenu_CharacterSelectionItemClickEventHandler(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                try
                {
                    var item = sender as ToolStripMenuItem;
                    var charId = (uint)item.Tag;
                    if (!item.Checked)
                    {
                        API.State.CameraControl = CameraControl.Character;
                    }

                    API.State.PlayerInfo[charId].IsTrackedByCamera = !item.Checked;

                    this.OverlayTitleContextMenuStrip.Show();
                    this.selectCharactersToolStripMenuItem.ShowDropDown();
                }
                catch (Exception ex)
                {
                    Program.WriteLog(ex);
                }
            }
        }

        private void exitOverlayModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.OverlayModeToolStripMenuItem.PerformClick();
        }

        private void optionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.ShowOptionsDialog();
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            if (API.State == null)
            {
                Program.WriteLog("MainWindow_Resize: Context.State is null.");
                return;
            }
            if (API.State.WindowMode != WindowMode.Overlay) { return; }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void followCharacter_Click(object sender, EventArgs e)
        {
            this.tileDisplay1.Focus();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SaveScreenShot();
        }

        private void CameraFollowCharacer_Click(object sender, EventArgs e)
        {
            API.State.CameraControl = CameraControl.Character;
        }

        private void MagnificationSlider_ValueChanged(object sender, EventArgs e)
        {
            var pos = API.Camera.RelativePosition();
            switch (this.MagnificationSlider.Value)
            {
                case -3: API.State.Magnification = 0.25f; break;
                case -2: API.State.Magnification = 0.50f; break;
                case -1: API.State.Magnification = 0.75f; break;
                case 0: API.State.Magnification = 1.00f; break;
                case 1: API.State.Magnification = 2.00f; break;
                case 2: API.State.Magnification = 4.00f; break;
            }

            this.magnificationLabel.Text = (API.State.Magnification * 100).ToString() + "%";
            
            API.Camera.AdjustScrollbarsToLayer();
            API.Camera.CenterOnRelativePosition(pos);
            this.tileDisplay1.Focus();
        }

        private void pluginManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PluginManagerForm.CreateShow();
        }

        #region Overlay ContextMenu: Map selection method
        private void rubikaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.mapSelectionControl1.RadioRK.Select();
        }

        private void shadowlandsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.mapSelectionControl1.RadioSL.Select();
        }

        private void autoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapSelectionControl1.RadioAuto.Select();
        }
        #endregion

        private void followCharactersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            API.State.CameraControl = CameraControl.Character;
        }

        private void pluginManagerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PluginManagerForm.CreateShow();
        }
    }

    public class MapSelectionItem
    {
        public string MapPath;
        public string MapName;
        public MapType Type;

        public override string ToString()
        {
            return this.MapName;
        }
    }
}