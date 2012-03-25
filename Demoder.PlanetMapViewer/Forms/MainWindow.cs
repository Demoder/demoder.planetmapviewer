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

        #endregion

        #region Form setup
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                this.Context = this.tileDisplay1.Context;

            }
            catch (Exception ex)
            {
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

                this.Context.ContentManager = this.tileDisplay1.Content;
                this.Context.HookInfo = new HookInfoTracker();
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

                this.bgwVersionCheck.DoWork += bgwVersionCheck_DoWork;
                this.bgwVersionCheck.RunWorkerCompleted += bgwVersionCheck_RunWorkerCompleted;

                this.Context.Content.Textures.CharacterLocator = this.Context.ContentManager.Load<Texture2D>(@"Textures\GFX_GUI_PLANETMAP_PLAYER_MARKER");
                this.ApplySettings();
                this.Context.Camera.AdjustScrollbarsToLayer();

                // Setup the tile display.
                Mouse.WindowHandle = this.tileDisplay1.Handle;
                this.tileDisplay1.OnDraw += tileDisplay1_OnDraw;
            }
            catch (Exception ex)
            {
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
            var res = VersionInfo.GetInfo("PlanetMapViewer", new Version(Application.ProductVersion));
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
                this.ToggleFullscreenSetting();
            }
            else
            {
                this.WindowState = windowSettings.WindowState;
                if (this.WindowState == FormWindowState.Normal)
                {
                    this.Width = windowSettings.WindowSize.X;
                    this.Height = windowSettings.WindowSize.Y;
                }
            }

            TileDisplay.FrameFrequency = 1000 / generalSettings.FramesPerSecond;
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
            switch (this.Context.Options.CameraControl)
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
                this.followCharacter.Invoke((Action)delegate()
                {
                    lock (this.Context.HookInfo.Processes)
                    {
                        foreach (var item in this.followCharacter.CheckedItems)
                        {
                            var info = item as AoInfo;
                            var charPos = this.Context.MapManager.GetPosition(info.Zone.ID, info.Position.X, info.Position.Z);
                            if (charPos == Vector2.Zero) { continue; }
                            vectors.Add(charPos);
                        }
                    }
                });
                if (vectors.Count == 0) { return; }

                this.Context.Camera.CenterOnVectors(vectors.ToArray());
            }
            catch (Exception ex)
            {
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

                #region Draw character locators

                if (this.Context.Content.Textures.CharacterLocator != null)
                {
                    // Retrieve all information related to character locators
                    var locators = this.GetCharacterLocators();

                    // Render the text
                    var strings = new List<StringDefinition>();
                    foreach (var loc in locators)
                    {
                        strings.AddRange(loc.Strings);
                    }
                    this.DrawText(strings);

                    // Render the markers
                    this.RenderCharacterLocators(locators);
                }
                #endregion
            }
        }

        private void DrawText(IEnumerable<StringDefinition> texts)
        {
            if (this.Context.SpriteBatch == null) { return; }
            if (this.Context.Content.Fonts.CharacterName == null) { return; }
            this.Context.SpriteBatch.Begin(
            SpriteSortMode.Texture,
            BlendState.AlphaBlend,
            null, null, null, null,
            this.Context.Camera.TransformMatrix);
            try
            {
                foreach (var sd in texts)
                {
                    var textSize = this.Context.Content.Fonts.CharacterName.MeasureString(sd.Text);
                    var fontPos = new Vector2(
                            (float)Math.Floor(sd.CenterPosition.X - (textSize.X / 2)),
                            (float)Math.Floor(sd.CenterPosition.Y + 2)
                            );
                    this.Context.SpriteBatch.DrawString(
                        this.Context.Content.Fonts.CharacterName,
                        sd.Text,
                        fontPos,
                        Microsoft.Xna.Framework.Color.Black
                        );
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Context.SpriteBatch.End();
            }

            this.Context.SpriteBatch.Begin(
                SpriteSortMode.Texture,
                BlendState.AlphaBlend,
                null, null, null, null,
                this.Context.Camera.TransformMatrix);
            try
            {


                foreach (var sd in texts)
                {
                    var textSize = this.Context.Content.Fonts.CharacterName.MeasureString(sd.Text);
                    var fontPos = new Vector2(
                            (float)Math.Floor(sd.CenterPosition.X - textSize.X / 2) - 1,
                            (float)Math.Floor(sd.CenterPosition.Y + 1)
                            );
                    this.Context.SpriteBatch.DrawString(
                        this.Context.Content.Fonts.CharacterName,
                        sd.Text,
                        fontPos,
                        Microsoft.Xna.Framework.Color.White
                        );
                }
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                this.Context.SpriteBatch.End();
            }
        }

        private void RenderCharacterLocators(CharacterLocatorInformation[] characters)
        {
            this.Context.SpriteBatch.Begin(
            SpriteSortMode.Texture,
            BlendState.AlphaBlend,
            null, null, null, null,
            this.Context.Camera.TransformMatrix);
            try
            {
                foreach (var c in characters)
                {
                    this.CenterTextureOnPixel(this.Context.Content.Textures.CharacterLocator, (int)c.CenterPosition.X, (int)c.CenterPosition.Y, Microsoft.Xna.Framework.Color.White);
                }
            }
            finally
            {
                this.Context.SpriteBatch.End();
            }
        }

        private CharacterLocatorInformation[] GetCharacterLocators()
        {
            var chrs = new List<CharacterLocatorInformation>();
            if (this.Context.HookInfo == null || this.Context.HookInfo.Processes == null)
            {
                return new CharacterLocatorInformation[0];
            }

            lock (this.Context.HookInfo.Processes)
            {
                foreach (var info in this.Context.HookInfo.Processes.Values)
                {
                    if (info == null || info.Zone == null || info.Position == null || info.Character == null || info.Character.Name == null) { continue; }
                    var charLoc = new CharacterLocatorInformation();
                    charLoc.CenterPosition = this.Context.MapManager.GetPosition(info.Zone.ID, info.Position.X, info.Position.Z);

                    charLoc.Strings.Add(new StringDefinition
                    {
                        CenterPosition = new Vector2(charLoc.CenterPosition.X, charLoc.CenterPosition.Y + (int)this.Context.Content.Textures.CharacterLocator.Height / 2),
                        Text = info.Character.Name
                    });

                    chrs.Add(charLoc);
                }
            }

            return chrs.ToArray();
        }

        private void CenterTextureOnPixel(Texture2D tex, int x, int y, Microsoft.Xna.Framework.Color color)
        {
            if (this.Context.SpriteBatch == null) { return; }
            this.Context.SpriteBatch.Draw(tex,
                    new Microsoft.Xna.Framework.Rectangle(
                        x - tex.Width / 2,
                        y - tex.Height / 2,
                        this.Context.Content.Textures.CharacterLocator.Width,
                        this.Context.Content.Textures.CharacterLocator.Height),
                         color);
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
            while (File.Exists(String.Format(nameFormat, num)))
            {
                num++;
            }

            bmpScreenshot.Save(String.Format(nameFormat, num), ImageFormat.Png);
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
            ToggleFullscreenSetting();
        }
        #endregion

        private void tileDisplay1_ScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            this.RadioButtonCameraManual.Checked = true;
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
            this.Context.Options.IsMapRubika = this.RadioButtonMapSelectionRubika.Checked;
            if (this.Context.MapManager == null) { return; }
            this.Context.UiElements.VScrollBar.Value = 0;
            this.Context.UiElements.HScrollBar.Value = 0;

            var mapInfo = (this.mapComboBox.SelectedItem as MapSelectionItem);
            if (this.Context.Options.IsMapRubika)
            {
                Properties.MapSettings.Default.SelectedRubikaMap = mapInfo.MapPath;
            }
            else
            {
                Properties.MapSettings.Default.SelectedShadowlandsMap = mapInfo.MapPath;
            }

            this.Context.MapManager.SelectMap(mapInfo.MapPath);
            this.Context.Camera.AdjustScrollbarsToLayer();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new AboutBox1();
            a.ShowDialog();
        }

        private BackgroundWorker bgwVersionCheck = new BackgroundWorker();
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
            this.Context.Options.IsMapRubika = this.RadioButtonMapSelectionRubika.Checked;
            if (this.Context.MapManager == null) { return; }
            this.Context.MapManager.FindAvailableMaps(this.Context.Options.IsMapRubika);
            if (this.Context.Options.IsMapRubika)
            {
                this.Context.MapManager.SelectRubikaMap();
            }
            else
            {
                this.Context.MapManager.SelectShadowlandsMap();
            }
        }

        private void ButtonZoomIn_Click(object sender, EventArgs e)
        {
            this.tileDisplay1.ZoomIn();
        }

        private void ButtonZoomOut_Click(object sender, EventArgs e)
        {
            this.tileDisplay1.ZoomOut();
        }

        private void RadioButtonCameraControlCheckChanged(object sender, EventArgs e)
        {
            this.ToggleCameraControl();
        }

        private void ToggleCameraControl()
        {
            if (this.RadioButtonCameraFollowCharacters.Checked)
            {
                this.Context.Options.CameraControl = CameraControl.Character;
            }
            else if (this.RadioButtonCameraManual.Checked)
            {
                this.Context.Options.CameraControl = CameraControl.Manual;
            }
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
                //this.menuStrip1.Visible = false;
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
                if (!Properties.GeneralSettings.Default.IgnoreOverlaymodeWarning)
                {
                    var res = MessageBox.Show("You are about to enter overlay mode.\r\nTo exit overlay mode, press [F12] or click the 'close window' button in the title bar.\r\n\r\nDo you wish to ignore this notice in the future?", "Overlay Mode", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (res == System.Windows.Forms.DialogResult.Cancel)
                    {
                        this.OverlayModeToolStripMenuItem.Checked = false;
                        return;
                    }
                    if (res == System.Windows.Forms.DialogResult.Yes)
                    {
                        Properties.GeneralSettings.Default.IgnoreOverlaymodeWarning = true;
                        Properties.GeneralSettings.Default.Save();
                    }
                }
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

                this.fullscreenToolStripMenuItem.Checked = false;
                ToggleFullscreenSetting();
                this.splitContainer1.SplitterDistance = this.splitContainer1.Width;
                this.menuStrip1.Visible = false;
                this.statusStrip1.Visible = false;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;

                this.MinimumSize = new Size(100, 100);

                this.tileDisplay1_hScrollBar.Visible = Properties.WindowSettings.Default.OverlaymodeShowScrollbars;
                this.tileDisplay1_vScrollBar.Visible = Properties.WindowSettings.Default.OverlaymodeShowScrollbars;

                this.ControlBox = Properties.WindowSettings.Default.OverlaymodeShowControlbox;
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

                this.splitContainer1.SplitterDistance = this.Width - 200;
                this.menuStrip1.Visible = true;
                this.statusStrip1.Visible = true;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;

                this.tileDisplay1_hScrollBar.Visible = true;
                this.tileDisplay1_vScrollBar.Visible = true;

                this.ControlBox = true;
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
            this.RadioButtonCameraFollowCharacters.PerformClick();
        }

        private void manualToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.RadioButtonCameraManual.PerformClick();
        }

        private void rubikaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.RadioButtonMapSelectionRubika.PerformClick();
        }

        private void shadowlandsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.RadioButtonMapSelectionShadowlands.PerformClick();
        }



        private void OverlayTitleContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            lock (this.OverlayTitleContextMenuStrip)
            {
                // Camera Controls
                this.rubikaToolStripMenuItem1.Checked = this.RadioButtonMapSelectionRubika.Checked;
                this.shadowlandsToolStripMenuItem1.Checked = this.RadioButtonMapSelectionShadowlands.Checked;

                // Region
                this.followCharactersToolStripMenuItem1.Checked = this.RadioButtonCameraFollowCharacters.Checked;
                this.manualToolStripMenuItem1.Checked = this.RadioButtonCameraManual.Checked;

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
                        catch { }
                    }
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
                        this.RadioButtonCameraFollowCharacters.Select();
                    }
                    this.followCharacter.SetItemChecked(this.followCharacter.Items.IndexOf(item.Tag), !item.Checked);

                    this.OverlayTitleContextMenuStrip.Show();
                    this.selectCharactersToolStripMenuItem.ShowDropDown();
                }
                catch { }
            }
        }

        private void exitOverlayModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.OverlayModeToolStripMenuItem.PerformClick();
        }

        private void optionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.optionsToolStripMenuItem.PerformClick();
        }
    }

    public class MapSelectionItem
    {
        public string MapPath;
        public string MapName;

        public override string ToString()
        {
            return this.MapName;
        }
    }
}