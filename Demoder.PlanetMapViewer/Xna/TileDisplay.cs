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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Demoder.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Demoder.PlanetMapViewer.Xna;
using Demoder.PlanetMapViewer.DataClasses;

namespace Demoder.PlanetMapViewer.Xna
{
    public class TileDisplay : GraphicsDeviceControl
    {
        /// <summary>
        /// Delay (in milliseconds) between frames.
        /// </summary>
        public static int FrameFrequency = 33;
        /// <summary>
        /// Used to limit FPS of the tileDisplay.
        /// </summary>
        private Stopwatch timeSinceLastDraw = Stopwatch.StartNew();

        private float mouseScrollSensitivity = 1;

        /// <summary>
        /// Determines wether or not the user is panning the map
        /// </summary>
        private bool isPanningMap = false;
        private Vector2 mousePosition = Vector2.Zero;

        public event EventHandler OnDraw;
        public event EventHandler OnInitialize;
        internal Context Context = new Context();

        private object drawLocker = new Object();

        #region Constructor / Initialization
        protected override void Initialize()
        {
            if (this.OnInitialize != null)
            {
                this.OnInitialize(this, null);
            }

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            try
            {
                this.Context.ContentManager = new ContentManager(Services, "Content");
                // Load textures
                this.Context.Content.Textures.CharacterLocator = this.Context.ContentManager.Load<Texture2D>(@"Textures\GFX_GUI_PLANETMAP_PLAYER_MARKER");
                this.Context.Content.Textures.MissionLocator = this.Context.ContentManager.Load<Texture2D>(@"Textures\GFX_GUI_PLANETMAP_MISSION_MARKER2");
                this.Context.Content.Textures.ArrowUp = this.Context.ContentManager.Load<Texture2D>(@"Textures\ArrowUp");
                this.Context.Content.Textures.TutorialFrame = this.Context.ContentManager.Load<Texture2D>(@"Textures\TutorialFrame");

                // Load fonts
                this.Context.Content.Fonts.CharacterName = this.Context.ContentManager.Load<SpriteFont>(@"Fonts\CharacterName");
                this.Context.Content.Fonts.GuiSmall = this.Context.ContentManager.Load<SpriteFont>(@"Fonts\GuiSmall");
                this.Context.Content.Fonts.GuiNormal = this.Context.ContentManager.Load<SpriteFont>(@"Fonts\GuiNormal");
                this.Context.Content.Fonts.GuiLarge = this.Context.ContentManager.Load<SpriteFont>(@"Fonts\GuiLarge");
                this.Context.Content.Fonts.GuiXLarge = this.Context.ContentManager.Load<SpriteFont>(@"Fonts\GuiXLarge");

                this.Context.Content.Loaded = true;
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.InvalidateFrame));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            base.OnInvalidated(e);
        }

        private void InvalidateFrame(object state)
        {
            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;
            do
            {
                var sw = Stopwatch.StartNew();
                try
                {
                    this.Invalidate();
                    var toSleep = (int)((1000 / TileDisplay.FrameFrequency) - sw.ElapsedMilliseconds);
                    if (toSleep > 0)
                    {
                        Thread.Sleep(toSleep);
                        sw.Restart();
                    }
                }
                catch (Exception ex)
                {
                    this.Context.ErrorLog.Enqueue(ex.ToString());
                }
            } while (true);
        }

        protected override void Draw()
        {
            try
            {
                lock (this.drawLocker)
                {
                    if (this.OnDraw != null)
                    {
                        var curSwVal = this.timeSinceLastDraw.ElapsedMilliseconds;
                        this.timeSinceLastDraw.Restart();

                        this.OnDraw(this, null);
#if DEBUG
                        {
                            var items = new List<IMapItem>();
                            items.Add(new MapText
                            {
                                Position = new Vector2(10, 10),
                                PositionAlignment = MapItemAlignment.Top | MapItemAlignment.Left,
                                Font = this.Context.Content.Fonts.GuiSmall,
                                Text = String.Format("FPS: {0}", 1000 / (this.timeSinceLastDraw.ElapsedMilliseconds + curSwVal)),
                                TextColor = Color.Pink,
                                ShadowColor = Color.Purple

                            });
                            this.Context.FrameDrawer.Draw(items.ToArray(), DrawMode.ViewPort);
                        }
#endif
                    }
                }
            }
            catch (Exception ex)
            {
                this.Context.ErrorLog.Enqueue(ex.ToString());
            }
        }

        #region Win32 API
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.HWnd != this.Handle)
            {
                return;
            }
            if (m.Msg == Win32MessageCodes.WM_MOUSEHWHEEL)
            {
                this.OnHorizontalMouseWheel(this, m.WParam.ToInt32());
                m.Result = (IntPtr)1;
            }

        }
        #endregion

        #region Mouse scrollwheel
        private void OnHorizontalMouseWheel(object sender, int value)
        {
            if (value == 0) { return; }
            if (this.Context == null) { return; }
            if (this.Context.Camera == null) { return; }

            if (this.Context.State.CameraControl != CameraControl.Manual)
            {
                this.Context.UiElements.ParentForm.RadioButtonCameraManual.Checked = true;
            }

            float newPos = this.Context.Camera.Center.X;
            if (value > 0)
            {
                newPos += this.Context.UiElements.HScrollBar.SmallChange * this.mouseScrollSensitivity;
            }
            else if (value < 0)
            {
                newPos -= this.Context.UiElements.HScrollBar.SmallChange * this.mouseScrollSensitivity;
            }
            this.Context.Camera.CenterOnPixel(newPos, this.Context.Camera.Center.Y);
            this.ReportMousePosition();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (this.Context == null) { return; }
            var button = e.Button;
            if (!this.Context.UiElements.ParentForm.RadioButtonCameraManual.Checked)
            {
                this.Context.UiElements.ParentForm.RadioButtonCameraManual.Checked = true;
            }
            if (ModifierKeys == System.Windows.Forms.Keys.Shift)
            {
                var newPos = (this.Context.Camera.Center.X - (e.Delta * this.mouseScrollSensitivity / 120 * this.Context.UiElements.HScrollBar.SmallChange));
                this.Context.Camera.CenterOnPixel(newPos, this.Context.Camera.Center.Y);
            }
            else
            {
                var newPos = (this.Context.Camera.Center.Y - (e.Delta * this.mouseScrollSensitivity / 120 * this.Context.UiElements.VScrollBar.SmallChange));
                this.Context.Camera.CenterOnPixel(this.Context.Camera.Center.X, newPos);
            }

            this.ReportMousePosition();
            base.OnMouseWheel(e);
        }
        #endregion

        #region Mouse clicking
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (this.Context == null) { return; }
            switch (e.Button)
            {
                case System.Windows.Forms.MouseButtons.Left:
                    this.Context.Camera.CenterOnPixel(
                        (int)this.Context.Camera.Position.X + e.X,
                        (int)this.Context.Camera.Position.Y + e.Y);
                    this.ZoomIn();                    
                    break;
                case System.Windows.Forms.MouseButtons.Right:
                    this.Context.Camera.CenterOnPixel(
                        (int)this.Context.Camera.Position.X + e.X,
                        (int)this.Context.Camera.Position.Y + e.Y);
                    this.ZoomOut();                    
                    break;
            }
            base.OnMouseDoubleClick(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (this.Context == null) { return; }
            switch (e.Button)
            {
                case System.Windows.Forms.MouseButtons.Middle:
                    this.Context.Camera.CenterOnPixel(
                        (int)this.Context.Camera.Position.X + e.X,
                        (int)this.Context.Camera.Position.Y + e.Y);
                    break;
            }
            base.OnMouseClick(e);
        }

       
        #endregion

        #region Mouse movement


        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (this.Context == null) { return; }
            if (this.Context.Camera == null) { return; }
            if (this.mousePosition == Vector2.Zero)
            {
                this.mousePosition.X = e.X;
                this.mousePosition.Y = e.Y;
            }
            var matrix = this.Context.Camera.TransformMatrix;
            this.ReportMousePosition();

            var mouseState = Mouse.GetState();
            if (!this.isPanningMap) { return; }
            var deltaX = e.X - this.mousePosition.X;
            var deltaY = e.Y - this.mousePosition.Y;
            // Are we in manual control mode?
            if (!this.Context.UiElements.ParentForm.RadioButtonCameraManual.Checked)
            {
                // If we're not already in manual control mode, require some delta. 
                // Prevents accidential scrolling.
                if (Math.Abs(deltaX) > 15 || Math.Abs(deltaY) > 15)
                {
                    this.Context.UiElements.ParentForm.RadioButtonCameraManual.Checked = true;
                }
                // Otherwise, discard the scroll.
                else
                {
                    return;
                }
            }
            this.mousePosition.X = e.X;
            this.mousePosition.Y = e.Y;

            var camPos = this.Context.Camera.Center;
            this.Context.Camera.CenterOnPixel(
                (int)(camPos.X - deltaX),
                (int)(camPos.Y - deltaY));

            base.OnMouseMove(e);
        }

        private void ReportMousePosition()
        {
            var matrix = this.Context.Camera.TransformMatrix;
            var mouseState = Mouse.GetState();
            this.Context.UiElements.ParentForm.ToolStripStatusLabel1.Text = String.Format(
                "Mouse: {0}x{1}",
                mouseState.X - matrix.Translation.X,
                mouseState.Y - matrix.Translation.Y);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            var mouseState = Mouse.GetState();
            if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                this.mousePosition.X = mouseState.X;
                this.mousePosition.Y = mouseState.Y;
                this.isPanningMap = true;
                this.Focus();
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
             var mouseState = Mouse.GetState();
            if (mouseState.LeftButton != Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                this.mousePosition = Vector2.Zero;
                this.isPanningMap = false;
            }
            base.OnMouseUp(e);
        }
        #endregion


        protected override void OnResize(EventArgs e)
        {
            if (this.Context == null) { return; }
            if (this.Context.Camera == null) { return; }
            //this.Invalidate();            
            this.Context.Camera.AdjustScrollbarsToLayer();
            this.ReportMousePosition();
            base.OnResize(e);
        }

        #region Keyboard input
        protected override void OnKeyDown(KeyEventArgs e)
        {
            this.HandleKeyDown(e);
            if (!e.SuppressKeyPress)
            {
                base.OnKeyDown(e);
            }
        }


        public void HandleKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.W:
                case System.Windows.Forms.Keys.A:
                case System.Windows.Forms.Keys.S:
                case System.Windows.Forms.Keys.D:
                    this.PanMap(e);
                    break;
                case System.Windows.Forms.Keys.Oemplus:
                case System.Windows.Forms.Keys.Add:
                    e.SuppressKeyPress = true;
                    this.ZoomIn();
                    break;
                case System.Windows.Forms.Keys.OemMinus:
                case System.Windows.Forms.Keys.Subtract:
                    e.SuppressKeyPress = true;
                    this.ZoomOut();
                    return;
                case System.Windows.Forms.Keys.Zoom:
                    e.SuppressKeyPress = true;
                    if (!e.Control)
                    {
                        this.ZoomIn();
                    }
                    else
                    {
                        this.ZoomOut();
                    }
                    break;
            }
        }

        private void PanMap(KeyEventArgs e)
        {
            if (e.Control || e.Alt) { return; }
            if (this.Context == null) { return; }
            if (this.Context.Camera == null) { return; }
            var x = 0;
            var y = 0;
            if (e.KeyData == System.Windows.Forms.Keys.W)
            {
                y -= 1;
                e.SuppressKeyPress = true;
            }
            if (e.KeyData == System.Windows.Forms.Keys.S)
            {
                y += 1;
                e.SuppressKeyPress = true;
            }
            if (e.KeyData == System.Windows.Forms.Keys.A)
            {
                x -= 1;
                e.SuppressKeyPress = true;
            }
            if (e.KeyData == System.Windows.Forms.Keys.D)
            {
                x += 1;
                e.SuppressKeyPress = true;
            }

            if (e.SuppressKeyPress)
            {
                this.Context.Camera.CenterOnPixel(
                    this.Context.Camera.Center.X + this.Context.UiElements.HScrollBar.SmallChange * x,
                    this.Context.Camera.Center.Y + this.Context.UiElements.VScrollBar.SmallChange * y);
                this.ReportMousePosition();
                return;
            }

            base.OnKeyDown(e);
        }
        #endregion

        public void ZoomIn()
        {
            if (this.Context == null) { return; }

            this.TutorialZoomIn();
            if (this.Context.MapManager == null) { return; }
            this.Context.MapManager.ZoomIn();
            this.ReportMousePosition();
        }

        public void ZoomOut()
        {
            if (this.Context == null) { return; }
            this.TutorialZoomOut();
            if (this.Context.MapManager == null) { return; }
            this.Context.MapManager.ZoomOut();
            this.ReportMousePosition();
        }

        private void TutorialZoomIn()
        {
            if (this.Context.State.WindowMode == WindowMode.Overlay) { return; }
            if (this.Context.Tutorial.Normal.CurrentStage == NormalTutorialStage.ZoomIn)
            {
                Properties.NormalTutorial.Default.ZoomIn = true;
                Properties.NormalTutorial.Default.Save();
            }
        }

        private void TutorialZoomOut()
        {
            if (this.Context.State.WindowMode == WindowMode.Overlay) { return; }
            if (this.Context.Tutorial.Normal.CurrentStage == NormalTutorialStage.ZoomOut)
            {
                Properties.NormalTutorial.Default.ZoomOut = true;
                Properties.NormalTutorial.Default.Save();
            }
        }
    }
}
