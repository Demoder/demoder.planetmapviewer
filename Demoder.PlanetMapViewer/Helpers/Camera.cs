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
using Microsoft.Xna.Framework;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using Demoder.PlanetMapViewer.DataClasses;

namespace Demoder.PlanetMapViewer.Helpers
{
    public class Camera
    {
        #region Members
        private Vector2 centerPosition = Vector2.Zero;
        public Matrix TransformMatrix { get { return Matrix.CreateTranslation(new Vector3(-this.Position, 0f)); } }

        #endregion

        public Vector2 Center
        {
            get 
            {
                return new Vector2(this.centerPosition.X, this.centerPosition.Y);
            }
        }

        public Vector2 Position
        {
            get
            {
                int x = (int)(this.centerPosition.X - API.GraphicsDevice.Viewport.Width / 2);
                int y = (int)(this.centerPosition.Y - API.GraphicsDevice.Viewport.Height / 2);
                return new Vector2(x, y);
            }
        }

        public Camera()
        {
        }

        #region Move camera

        public void CenterOnPixel(float x, float y)
        {
            this.CenterOnPixel((int)x, (int)y);
        }

        public void CenterOnPixel(int x, int y)
        {
            if (API.MapManager == null) { return; }
            if (API.MapManager.CurrentLayer == null) { return; }

            if (API.UiElements.HScrollBar.InvokeRequired)
            {
                API.UiElements.HScrollBar.Invoke((Action)delegate()
                {
                    this.RealCenterOnPixel(x, y);
                });
            }
            else
            {
                RealCenterOnPixel(x, y);
            }
        }

        private void RealCenterOnPixel(int x, int y)
        {
            lock (this)
            {
                // Sanitize numbers
                x = Math.Max(x, 0);
                y = Math.Max(y, 0);
                x = (int)Math.Min(x, API.MapManager.CurrentLayer.Size.X * API.State.Magnification);
                y = (int)Math.Min(y, API.MapManager.CurrentLayer.Size.Y * API.State.Magnification);

                // Set position
                this.centerPosition.X = (int)Math.Floor((float)x);
                this.centerPosition.Y = (int)Math.Floor((float)y);

                // Update scrollbars!
                var pos = this.Position;
                try
                {
                    API.UiElements.HScrollBar.Value = (int)pos.X;
                    API.UiElements.VScrollBar.Value = (int)pos.Y;
                }
                catch (ArgumentOutOfRangeException ex)
                {

                }
                catch (Exception ex)
                {
                    Program.WriteLog(ex);
                }
            }
        }

        public void CenterOnVector(Vector2 vector)
        {
            this.CenterOnPixel((int)vector.X, (int)vector.Y);
        }

        public void CenterOnVectors(Vector2[] vectors)
        {
            var vec = Vector2.Zero;
            foreach (var vector in vectors)
            {
                vec += vector;
            }

            vec = new Vector2(
                vec.X / vectors.Length,
                vec.Y / vectors.Length);

            this.CenterOnVector(vec);
        }

        public void CenterOnScrollbars()
        {
            var x = API.UiElements.HScrollBar.Value + API.GraphicsDevice.Viewport.Width / 2;
            var y = API.UiElements.VScrollBar.Value + API.GraphicsDevice.Viewport.Height / 2;
            this.CenterOnPixel(x, y);
        }
  
        public void CenterOnRelativePosition(Vector2 relativePosition)
        {
            if (API.MapManager == null) { return; }
            if (API.MapManager.CurrentMap == null) { return; }
            //var pfId = this.mapManager.CurrentMap.CoordsFile.Playfields.First(pf=>pf.XScale==1 && pf.YScale==1).ID;
            var pfId = API.MapManager.CurrentMap.CoordsFile.Playfields[0].ID;
            var centerPos = API.MapManager.GetPosition(pfId, relativePosition.X,relativePosition.Y);
            this.CenterOnVector(centerPos);
        }

        #endregion

        public Vector2 RelativePosition()
        {
            if (API.MapManager == null) { return default(Vector2); }
            if (API.MapManager.CurrentMap == null) { return default(Vector2); }

            //var pfId = this.mapManager.CurrentMap.CoordsFile.Playfields.First(pf => pf.XScale == 1 && pf.YScale == 1).ID;
            var pfId = API.MapManager.CurrentMap.CoordsFile.Playfields[0].ID;
            return API.MapManager.GetReversePosition(pfId, this.Center.X, this.Center.Y);
        }
       
        internal void AdjustScrollbarsToLayer()
        {
            if (API.UiElements.TileDisplay == null) { return; }
            if (API.UiElements.HScrollBar == null) { return; }
            if (API.UiElements.VScrollBar == null) { return; }
            if (API.MapManager == null) { return; }
            if (API.MapManager.CurrentLayer == null) { return; }

            #region Horizontal scrollbar
            int horModifier = API.UiElements.TileDisplay.Width / 2;
            API.UiElements.HScrollBar.Maximum = (int)(API.MapManager.CurrentLayer.Size.X * API.State.Magnification) + horModifier;
            API.UiElements.HScrollBar.Minimum = -horModifier;

            API.UiElements.HScrollBar.LargeChange = API.UiElements.TileDisplay.Width;
            API.UiElements.HScrollBar.SmallChange = API.UiElements.TileDisplay.Width / 10;
            API.UiElements.HScrollBar.Invalidate();
            #endregion

            #region Vertical scrollbar
            var verModifier = API.UiElements.TileDisplay.Height / 2;
            API.UiElements.VScrollBar.Maximum = (int)(API.MapManager.CurrentLayer.Size.Y * API.State.Magnification)+ verModifier;
            API.UiElements.VScrollBar.Minimum = -verModifier;

            API.UiElements.VScrollBar.LargeChange = API.UiElements.TileDisplay.Height;
            API.UiElements.VScrollBar.SmallChange = API.UiElements.TileDisplay.Height / 10;
            API.UiElements.VScrollBar.Invalidate();
            #endregion
        }
    }
}
