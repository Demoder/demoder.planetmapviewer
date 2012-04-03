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
using System.Drawing;
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

namespace Demoder.PlanetMapViewer.Forms
{
    public partial class MainWindow : Form
    {
        #region Members
        #region  XNA stuff
        internal Context Context;
        #endregion
        private Stopwatch lastException;

        #region Timers
        private thrd.Timer updateCharacterListTimer;
        private thrd.Timer topMostTimer;
        #endregion

        private string screenShotFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Demoders PlanetMap Viewer");
        private BackgroundWorker bgwVersionCheck = new BackgroundWorker();
        #endregion

        #region Form setup
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                this.splitContainer1.FixedPanel = FixedPanel.Panel2;
                this.splitContainer1.SplitterDistance = 605;
                this.Context = this.tileDisplay1.Context;
            }
            catch (Exception ex)
            {
                this.Context.ErrorLog.Enqueue(ex.ToString());
                this.ShowExceptionError(ex);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                this.Context.UiElements.TileDisplay = this.tileDisplay1;
                this.Context.UiElements.HScrollBar = this.tileDisplay1_hScrollBar;
                this.Context.UiElements.VScrollBar = this.tileDisplay1_vScrollBar;
                this.Context.UiElements.MapList = this.mapComboBox;
                this.Context.UiElements.ParentForm = this;

                this.Context.HookInfo = new HookInfoTracker(this.Context);
                this.updateCharacterListTimer = new thrd.Timer(this.UpdateCharacterList, null, 1000, 2000);

                // Check if we should attempt to upgrade settings
                if (Properties.GeneralSettings.Default.SettingVersion != this.ProductVersion.ToString())
                {
                    Properties.GeneralSettings.Default.Upgrade();
                    Properties.MapSettings.Default.Upgrade();
                    Properties.WindowSettings.Default.Upgrade();
                    Properties.GeneralSettings.Default.SettingVersion = this.ProductVersion.ToString();
                    Properties.GeneralSettings.Default.Save();
                }
                if (!OptionWindow.IsValidAoFolder(Properties.MapSettings.Default.AoPath))
                {
                    var dr = this.ShowOptionsDialog("Demoder's Planet Map Viewer - Options");
                    if (dr == DialogResult.Cancel)
                    {
                        Application.Exit();
                        return;
                    }
                }

                this.Context.SpriteBatch = new SpriteBatch(this.Context.GraphicsDevice);
                this.Context.MapManager = new MapManager(this.Context);
                this.Context.Camera = new Camera(this.Context);
                this.Context.MapManager.Initialize();
                

                this.bgwVersionCheck.DoWork += bgwVersionCheck_DoWork;
                this.bgwVersionCheck.RunWorkerCompleted += bgwVersionCheck_RunWorkerCompleted;
                this.ApplySettings();
                this.Context.Camera.AdjustScrollbarsToLayer();

                // Setup the tile display.
                Mouse.WindowHandle = this.tileDisplay1.Handle;
                this.tileDisplay1.OnDraw += tileDisplay1_OnDraw;

#if DEBUG
                this.errorLogToolStripMenuItem.Visible = true;
#else 
                this.errorLogToolStripMenuItem.Visible = false;
#endif

            }
            catch (Exception ex)
            {
                this.Context.ErrorLog.Enqueue(ex.ToString());
                this.ShowExceptionError(ex);
                Application.Exit();
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
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
            Properties.MapSettings.Default.Save();
            Properties.WindowSettings.Default.Save();
        }
        #endregion

        #region Version chcecking
        private void bgwVersionCheck_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result == null)
            {
                MessageBox.Show("Unexpected server response, try again later.", "Version information", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                this.Context.State.WindowMode = WindowMode.Fullscreen;
                this.ToggleFullscreenSetting();
            }
            else
            {
                this.WindowState = windowSettings.WindowState;
                this.Context.State.WindowMode = WindowMode.Normal;
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

        private void UpdateCharacterList(object state)
        {
            if (this.followCharacter.InvokeRequired)
            {
                this.followCharacter.Invoke((Action)this.UpdateCharacterListDoWork);
            }
            else
            {
                this.UpdateCharacterListDoWork();
            }
        }

        private void UpdateCharacterListDoWork()
        {
            if (this.Context == null) { return; }
            if (this.Context.HookInfo == null) { return; }
            if (this.Context.HookInfo.Processes == null) { return; }

            lock (this.Context.HookInfo.Processes)
            {
                this.followCharacter.BeginUpdate();
                // Remove entries from the list if they're no longer tracked
                var toRemove = new List<AoInfo>();
                // Find which ones to remove
                foreach (var oldInfo in this.followCharacter.Items)
                {
                    if (oldInfo == null) { continue; }
                    var aoInfo = oldInfo as AoInfo;
                    if (!this.Context.HookInfo.Processes.ContainsKey(aoInfo.ProcessID))
                    {
                        toRemove.Add(aoInfo);
                    }
                }
                // Now remove them
                foreach (var aoInfo in toRemove)
                {
                    this.followCharacter.Items.Remove(aoInfo);
                }

                // Add new entries, if any.
                foreach (var newInfo in this.Context.HookInfo.Processes.Values)
                {
                    if (newInfo == null) { continue; }
                    if (!this.followCharacter.Items.Contains(newInfo))
                    {
                        this.followCharacter.Items.Add(newInfo);
                        if (this.followCharacter.Items.Count == 1 && this.followCharacter.CheckedItems.Count == 0)
                        {
                            this.followCharacter.SetItemChecked(0, true);
                        }
                    }
                }

                this.followCharacter.EndUpdate();
            }
        }

        private void tileDisplay1_OnDraw(object sender, EventArgs e)
        {
            if (this.Context.Camera == null) { return; }
            if (this.Context.MapManager == null) { return; }
            if (this.Context.MapManager.CurrentLayer == null) { return; }
            this.Logic();
            this.Render();
        }


        private void Logic()
        {
            if (this.Context.Camera == null) { return; }
            switch (this.Context.State.CameraControl)
            {
                case CameraControl.Character:
                    this.MoveCameraToCharacter();
                    break;
                case CameraControl.Manual:
                    this.Context.Camera.CenterOnScrollbars();
                    break;
            }
        }

        #region Camera controls
        private void MoveCameraToCharacter()
        {
            try
            {
                int textureSize = this.Context.MapManager.CurrentLayer.TextureSize;
                var vectors = new List<Vector2>();
                int shadowlandsCharacters = 0;
                int rubikaCharacters = 0;
                this.followCharacter.Invoke((Action)delegate()
                {
                    lock (this.Context.HookInfo.Processes)
                    {
                        foreach (var item in this.followCharacter.CheckedItems)
                        {
                            var info = item as AoInfo;

                            // Track rk/sl characters
                            if (info.Character.InShadowlands)
                            {
                                shadowlandsCharacters++;
                            }
                            else
                            {
                                rubikaCharacters++;
                            }

                            if (info.Character.InShadowlands && this.Context.MapManager.CurrentMap.Type != MapType.Shadowlands) { continue; }

                            var charPos = this.Context.MapManager.GetPosition(info.Character.Zone.ID, info.Character.Position.X, info.Character.Position.Z);
                            if (charPos == Vector2.Zero) { continue; }
                            vectors.Add(charPos);
                        }
                        if (this.Context.State.MapTypeAutoSwitching)
                        {
                            if (rubikaCharacters > shadowlandsCharacters && this.Context.MapManager.CurrentMap.Type != MapType.Rubika)
                            {
                                this.Context.MapManager.FindAvailableMaps(MapType.Rubika);
                                this.Context.MapManager.SelectMap(MapType.Rubika);
                            }
                            else if (shadowlandsCharacters > rubikaCharacters && this.Context.MapManager.CurrentMap.Type != MapType.Shadowlands)
                            {
                                this.Context.MapManager.FindAvailableMaps(MapType.Shadowlands);
                                this.Context.MapManager.SelectMap(MapType.Shadowlands);
                            }
                        }
                    }
                });
                if (vectors.Count == 0) { return; }

                this.Context.Camera.CenterOnVectors(vectors.ToArray());
            }
            catch (Exception ex)
            {
                this.Context.ErrorLog.Enqueue(ex.ToString());
                this.ShowExceptionError(ex);
            }
        }

        #endregion



        private void Render()
        {
            lock (this.Context.Camera)
            {
                this.Context.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);

                this.Context.MapManager.CurrentLayer.Draw(this.Context);
                if (!this.Context.Content.Loaded) { return; }

                #region Draw character locators
                // Retrieve all information related to character locators
                var mapItems = this.GetCharacterLocators();
                this.Context.FrameDrawer.Draw(mapItems);
                mapItems = this.GetMissionLocators();
                this.Context.FrameDrawer.Draw(mapItems);
                #endregion

                this.RenderTutorial();                
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if tutorial was rendered, false otherwise</returns>
        private bool RenderTutorial()
        {
            if (this.Context.State.WindowMode == WindowMode.Overlay)
            {
                if (this.Context.Tutorial.Overlay.CurrentStage == OverlayTutorialStage.Completed) { return false; }
                this.Context.Tutorial.Overlay.DrawTutorial();
                return true;
            }
            else
            {
                if (this.Context.Tutorial.Normal.CurrentStage == NormalTutorialStage.Completed) { return false; }
                this.Context.Tutorial.Normal.DrawTutorial();
                return true;
            }
        }

        private IMapItem[] GetCharacterLocators()
        {
            var mapItems = new List<IMapItem>();
            var mapTexts = new List<IMapItem>();
            if (this.Context.HookInfo == null || this.Context.HookInfo.Processes == null)
            {
                return new MapTexture[0];
            }

            lock (this.Context.HookInfo.Processes)
            {
                foreach (var info in this.Context.HookInfo.Processes.Values)
                {
                    if (info == null || info.Character == null || info.Character.Zone == null || info.Character.Position == null || info.Character.Name == null) { continue; }
                    var charLoc = new MapTexture();
                    charLoc.Texture = this.Context.Content.Textures.CharacterLocator;
                    charLoc.Position = this.Context.MapManager.GetPosition(info.Character.Zone.ID, info.Character.Position.X, info.Character.Position.Z);
                    charLoc.PositionAlignment = MapItemAlignment.Center;
                    
                    mapTexts.Add(new MapText
                    {
                        Position = new Vector2(charLoc.Position.X, charLoc.Position.Y + (int)charLoc.Texture.Height / 2 + 5),
                        Text = info.Character.Name,
                        TextColor = Microsoft.Xna.Framework.Color.White,
                        ShadowColor = Microsoft.Xna.Framework.Color.Black,
                        Shadow = true,
                        Font = this.Context.Content.Fonts.CharacterName
                    });

                    mapItems.Add(charLoc);
                }
            }
            mapItems.AddRange(mapTexts);
            return mapItems.ToArray();
        }

        private IMapItem[] GetMissionLocators()
        {
            var mapItems = new List<IMapItem>();
            var mapTexts = new List<IMapItem>();
            if (this.Context.HookInfo == null || this.Context.HookInfo.Processes == null)
            {
                return new MapTexture[0];
            }

            lock (this.Context.HookInfo.Processes)
            {
                foreach (var info in this.Context.HookInfo.Processes.Values)
                {
                    // We need both mission locator info and character name info
                    if (info == null) { continue; }
                    if (info.Mission == null || info.Mission.Zone == null || info.Mission.ZonePosition == null || info.Mission.ZonePosition == default(Vector3)) { continue; }
                    if (info.Character == null || info.Character.Name == null) { continue; }

                    var mapItem = new MapTexture();
                    mapItem.Texture = this.Context.Content.Textures.MissionLocator;
                    mapItem.Position = this.Context.MapManager.GetPosition(info.Mission.Zone.ID, info.Mission.ZonePosition.X, info.Mission.ZonePosition.Z);
                    
                    mapItem.PositionAlignment = MapItemAlignment.Bottom | MapItemAlignment.Center;
                    mapTexts.Add(new MapText
                    {
                        Position = new Vector2(mapItem.Position.X, mapItem.Position.Y + (int)mapItem.Texture.Height / 2 + 5),
                        Text = info.Character.Name,
                        TextColor = Microsoft.Xna.Framework.Color.White,
                        ShadowColor = Microsoft.Xna.Framework.Color.Black,
                        Shadow = true,
                        Font = this.Context.Content.Fonts.CharacterName
                    });

                    mapItems.Add(mapItem);
                }
            }
            mapItems.AddRange(mapTexts);
            return mapItems.ToArray();
        }

        FormWindowState oldState = FormWindowState.Normal;


        #region Misc features
        private void SaveScreenShot()
        {
            var bmpScreenshot = new Bitmap(this.Width, this.Height, PixelFormat.Format32bppArgb);
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            gfxScreenshot.CopyFromScreen(this.Left, this.Top, 0, 0, this.Size, CopyPixelOperation.SourceCopy);


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
                this.Context.State.WindowMode = WindowMode.Fullscreen;
            }
            else
            {
                this.Context.State.WindowMode = WindowMode.Normal;
            }
            this.ToggleFullscreenSetting();
        }
        #endregion

        private void tileDisplay1_ScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            this.Context.State.CameraControl = CameraControl.Manual;
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowOptionsDialog();
        }

        private DialogResult ShowOptionsDialog(string customTitle = null)
        {
            var options = new OptionWindow(this.Context);
            options.StartPosition = FormStartPosition.CenterParent;
            if (customTitle != null)
            {
                options.Text = customTitle;
            }
            return options.ShowDialog();
        }

        private void mapComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.Context.MapManager == null) { return; }
            this.Context.UiElements.VScrollBar.Value = 0;
            this.Context.UiElements.HScrollBar.Value = 0;

            var mapInfo = (this.mapComboBox.SelectedItem as MapSelectionItem);
            if (mapInfo == null) { return; }
            if (mapInfo.Type == MapType.Rubika)
            {
                Properties.MapSettings.Default.SelectedRubikaMap = mapInfo.MapPath;
            }
            else
            {
                Properties.MapSettings.Default.SelectedShadowlandsMap = mapInfo.MapPath;
            }

            this.Context.MapManager.SelectMap(mapInfo.MapPath);
            this.Context.Camera.AdjustScrollbarsToLayer();
            this.tileDisplay1.Focus();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new AboutBox1();
            a.ShowDialog();
        }

       
        private void checkVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.bgwVersionCheck.IsBusy)
            {
                MessageBox.Show("Already checking version!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            this.bgwVersionCheck.RunWorkerAsync();
        }

        private void RadioMapTypeCheckedChanged(object sender, EventArgs e)
        {
            // User clicking on a RadioButton will send two statechange messages.
            // One for the unchecked and one for the checked RadioButton.
            // We only need to process one of the events.
            if (!((RadioButton)sender).Checked)
            {
                return;
            }

            // Automatic selection
            if (this.RadioButtonMapSelectionAuto.Checked)
            {
                this.Context.State.MapTypeAutoSwitching = true;
                return;
            }

            // RK selected
            if (this.RadioButtonMapSelectionRubika.Checked)
            {
                this.Context.State.MapType = MapType.Rubika;
            }
            // SL selected
            else if (this.RadioButtonMapSelectionShadowlands.Checked)
            {
                this.Context.State.MapType = MapType.Shadowlands;
            }
            
            this.SelectMap();
            this.Context.State.MapTypeAutoSwitching = false;
            return;
        }

        private void SelectMap()
        {
            if (this.Context.MapManager == null) { return; }

            if (this.RadioButtonMapSelectionAuto.Checked == true)
            {
                return;
            }

            this.Context.MapManager.FindAvailableMaps(this.Context.State.MapType);
            this.Context.MapManager.SelectMap(this.Context.State.MapType);
            this.tileDisplay1.Focus();
        }

        private void ButtonZoomIn_Click(object sender, EventArgs e)
        {
            this.tileDisplay1.ZoomIn();
            this.tileDisplay1.Focus();
        }

        private void ButtonZoomOut_Click(object sender, EventArgs e)
        {
            this.tileDisplay1.ZoomOut();
            this.tileDisplay1.Focus();
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
            }

            else
            {
                this.WindowState = this.oldState;
                this.ControlBox = true;
                this.menuStrip1.Visible = true;
                this.statusStrip1.Visible = true;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                this.Padding = new Padding(0);
            }
        }

        internal void ToggleOverlayMode()
        {
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

                this.MinimumSize = new Size(100, 100);

                this.tileDisplay1_hScrollBar.Visible = Properties.WindowSettings.Default.OverlaymodeShowScrollbars;
                this.tileDisplay1_vScrollBar.Visible = Properties.WindowSettings.Default.OverlaymodeShowScrollbars;

                this.ControlBox = Properties.WindowSettings.Default.OverlaymodeShowControlbox;
                this.splitContainer1.Panel2Collapsed = true;
                this.Context.State.WindowMode = WindowMode.Overlay;

                if (this.Context.Tutorial.Normal.CurrentStage == NormalTutorialStage.OverlayMode)
                {
                    Properties.NormalTutorial.Default.OverlayMode = true;
                    Properties.NormalTutorial.Default.Save();
                }
            }
            else
            {
                this.MinimumSize = new Size(300, 300);
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
                this.Context.State.WindowMode = WindowMode.Normal;

                if (this.Context.Tutorial.Overlay.CurrentStage == OverlayTutorialStage.ExitOverlayMode)
                {
                    Properties.OverlayTutorial.Default.ExitOverlayMode = true;
                    Properties.OverlayTutorial.Default.Save();
                }
            }
        }

        private void ForceTopMost()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action)delegate()
                {
                    this.TopMost = false;
                    this.TopMost = true;
                    this.SetTopLevel(true);
                });
            }
            else
            {
                this.TopMost = false;
                this.TopMost = true;
                this.SetTopLevel(true);
            }
        }

        private void followCharactersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Context.State.CameraControl = CameraControl.Character;
        }

        private void missionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Context.State.CameraControl = CameraControl.Manual;
            this.MoveCameraToMission();
        }   
        private void rubikaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.RadioButtonMapSelectionRubika.Select();
        }

        private void shadowlandsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.RadioButtonMapSelectionShadowlands.Select();
        }
        private void autoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.RadioButtonMapSelectionAuto.Select();
        }


        private void OverlayTitleContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            lock (this.OverlayTitleContextMenuStrip)
            {
                if (this.Context.Tutorial.Overlay.CurrentStage == OverlayTutorialStage.TitlebarMenu)
                {
                    Properties.OverlayTutorial.Default.TitlebarMenu = true;
                    Properties.OverlayTutorial.Default.Save();
                }
                // Region
                this.rubikaToolStripMenuItem1.Checked = this.RadioButtonMapSelectionRubika.Checked;
                this.shadowlandsToolStripMenuItem1.Checked = this.RadioButtonMapSelectionShadowlands.Checked;
                this.autoToolStripMenuItem.Checked = this.RadioButtonMapSelectionAuto.Checked;

                // Camera controls
                this.followCharactersToolStripMenuItem1.Checked = this.Context.State.CameraControl == CameraControl.Character;

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
                    foreach (var i in this.mapComboBox.Items)
                    {
                        var map = i as MapSelectionItem;
                        var item = new ToolStripMenuItem(map.MapName);
                        item.Tag = map.MapPath;
                        item.Click += this.OverlayTitleContextMenu_MapSelectionItemClickEventHandler;
                        if (this.mapComboBox.SelectedItem == map)
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
                    var characters = new object[this.followCharacter.Items.Count];
                    this.followCharacter.Items.CopyTo(characters, 0);
                    foreach (AoInfo aoInfo in characters)
                    {
                        try
                        {
                            var item = new ToolStripMenuItem(aoInfo.Character.Name);
                            item.Tag = aoInfo;
                            item.Checked = this.followCharacter.CheckedItems.Contains(aoInfo);
                            item.Click += this.OverlayTitleContextMenu_CharacterSelectionItemClickEventHandler;
                            this.selectCharactersToolStripMenuItem.DropDownItems.Add(item);
                        }
                        catch(Exception ex) 
                        {
                            this.Context.ErrorLog.Enqueue(ex.ToString());
                        }
                    }
                    this.selectCharactersToolStripMenuItem.Enabled = this.selectCharactersToolStripMenuItem.DropDownItems.Count != 0;
                }
                #endregion
            }
        }

        void OverlayTitleContextMenu_MapSelectionItemClickEventHandler(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                var item = sender as ToolStripMenuItem;
                this.Context.MapManager.SelectMap(item.Tag.ToString());
            }
        }

        void OverlayTitleContextMenu_CharacterSelectionItemClickEventHandler(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                try
                {
                    var item = sender as ToolStripMenuItem;
                    if (!item.Checked)
                    {
                        this.Context.State.CameraControl = CameraControl.Character;
                    }
                    this.followCharacter.SetItemChecked(this.followCharacter.Items.IndexOf(item.Tag), !item.Checked);

                    this.OverlayTitleContextMenuStrip.Show();
                    this.selectCharactersToolStripMenuItem.ShowDropDown();
                }
                catch (Exception ex)
                {
                    this.Context.ErrorLog.Enqueue(ex.ToString());
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

        private void errorLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var err = new ErrorLog();
            var errors = this.Context.ErrorLog.ToArray();
            err.textBox1.Text = String.Join("\r\n\r\n", errors);
            err.ShowDialog();
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            if (this.Context.State.WindowMode != WindowMode.Overlay) { return; }
            if (this.Context.Tutorial.Overlay.CurrentStage == OverlayTutorialStage.ResizeWindow)
            {
                Properties.OverlayTutorial.Default.ResizeWindow = true;
                Properties.OverlayTutorial.Default.Save();
            }
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
            this.Context.State.CameraControl = CameraControl.Character;
        }

        private void CameraFollowMission_Click(object sender, EventArgs e)
        {
            this.Context.State.CameraControl = CameraControl.Manual;
            // Insert code to move camera to mission marker here.
            this.MoveCameraToMission();
        }

        private void MoveCameraToMission()
        {
            try
            {
                var vectors = new List<Vector2>();
                this.followCharacter.Invoke((Action)delegate()
                {
                    lock (this.Context.HookInfo.Processes)
                    {
                        foreach (var item in this.followCharacter.CheckedItems)
                        {
                            var info = item as AoInfo;

                            var questPos = this.Context.MapManager.GetPosition(info.Mission.Zone.ID, info.Mission.ZonePosition.X, info.Mission.ZonePosition.Z);
                            if (questPos == Vector2.Zero) { continue; }
                            vectors.Add(questPos);
                        }
                    }
                });
                if (vectors.Count == 0) { return; }

                this.Context.Camera.CenterOnVectors(vectors.ToArray());
            }
            catch (Exception ex)
            {
                this.Context.ErrorLog.Enqueue(ex.ToString());
                this.ShowExceptionError(ex);
            }
        
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            var pos = this.Context.Camera.RelativePosition();
            this.Context.State.Magnification = this.MagnificationSlider.Value / 100f;
            this.magnificationLabel.Text = this.MagnificationSlider.Value.ToString() + "%";
            this.Context.Camera.AdjustScrollbarsToLayer();
            this.Context.Camera.CenterOnRelativePosition(pos);
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