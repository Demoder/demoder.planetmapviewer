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
        public Context Context;

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
                int x = (int)(this.centerPosition.X - this.Context.GraphicsDevice.Viewport.Width / 2);
                int y = (int)(this.centerPosition.Y - this.Context.GraphicsDevice.Viewport.Height / 2);
                return new Vector2(x, y);
            }
        }

        public Camera(Context context)
        {
            this.Context = context;
        }

        #region Move camera

        public void CenterOnPixel(float x, float y)
        {
            this.CenterOnPixel((int)x, (int)y);
        }

        public void CenterOnPixel(int x, int y)
        {
            if (this.Context.MapManager == null) { return; }
            if (this.Context.MapManager.CurrentLayer == null) { return; }

            if (this.Context.UiElements.HScrollBar.InvokeRequired)
            {
                this.Context.UiElements.HScrollBar.Invoke((Action)delegate()
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
                x = (int)Math.Min(x, this.Context.MapManager.CurrentLayer.Size.X * this.Context.State.Magnification);
                y = (int)Math.Min(y, this.Context.MapManager.CurrentLayer.Size.Y * this.Context.State.Magnification);

                // Set position
                this.centerPosition.X = (int)Math.Floor((float)x);
                this.centerPosition.Y = (int)Math.Floor((float)y);

                // Update scrollbars!
                var pos = this.Position;
                try
                {
                    this.Context.UiElements.HScrollBar.Value = (int)pos.X;
                    this.Context.UiElements.VScrollBar.Value = (int)pos.Y;
                }
                catch (Exception ex)
                {
                    this.Context.ErrorLog.Enqueue(ex.ToString());
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
            var x = this.Context.UiElements.HScrollBar.Value + this.Context.GraphicsDevice.Viewport.Width / 2;
            var y = this.Context.UiElements.VScrollBar.Value + this.Context.GraphicsDevice.Viewport.Height / 2;
            this.CenterOnPixel(x, y);
        }
  
        public void CenterOnRelativePosition(Vector2 relativePosition)
        {
            if (this.Context.MapManager == null) { return; }
            if (this.Context.MapManager.CurrentMap == null) { return; }
            //var pfId = this.mapManager.CurrentMap.CoordsFile.Playfields.First(pf=>pf.XScale==1 && pf.YScale==1).ID;
            var pfId = this.Context.MapManager.CurrentMap.CoordsFile.Playfields[0].ID;
            var centerPos = this.Context.MapManager.GetPosition(pfId, relativePosition.X,relativePosition.Y);
            this.CenterOnVector(centerPos);
        }

        #endregion

        public Vector2 RelativePosition()
        {
            if (this.Context.MapManager == null) { return default(Vector2); }
            if (this.Context.MapManager.CurrentMap == null) { return default(Vector2); }

            //var pfId = this.mapManager.CurrentMap.CoordsFile.Playfields.First(pf => pf.XScale == 1 && pf.YScale == 1).ID;
            var pfId = this.Context.MapManager.CurrentMap.CoordsFile.Playfields[0].ID;
            return this.Context.MapManager.GetReversePosition(pfId, this.Center.X, this.Center.Y);
        }
       
        internal void AdjustScrollbarsToLayer()
        {
            if (this.Context.UiElements.TileDisplay == null) { return; }
            if (this.Context.UiElements.HScrollBar == null) { return; }
            if (this.Context.UiElements.VScrollBar == null) { return; }
            if (this.Context.MapManager == null) { return; }
            if (this.Context.MapManager.CurrentLayer == null) { return; }

            #region Horizontal scrollbar
            int horModifier = this.Context.UiElements.TileDisplay.Width / 2;
            this.Context.UiElements.HScrollBar.Maximum = (int)(this.Context.MapManager.CurrentLayer.Size.X * this.Context.State.Magnification) + horModifier;
            this.Context.UiElements.HScrollBar.Minimum = -horModifier;

            this.Context.UiElements.HScrollBar.LargeChange = this.Context.UiElements.TileDisplay.Width;
            this.Context.UiElements.HScrollBar.SmallChange = this.Context.UiElements.TileDisplay.Width / 10;
            this.Context.UiElements.HScrollBar.Invalidate();
            #endregion

            #region Vertical scrollbar
            var verModifier = this.Context.UiElements.TileDisplay.Height / 2;
            this.Context.UiElements.VScrollBar.Maximum = (int)(this.Context.MapManager.CurrentLayer.Size.Y * this.Context.State.Magnification)+ verModifier;
            this.Context.UiElements.VScrollBar.Minimum = -verModifier;

            this.Context.UiElements.VScrollBar.LargeChange = this.Context.UiElements.TileDisplay.Height;
            this.Context.UiElements.VScrollBar.SmallChange = this.Context.UiElements.TileDisplay.Height / 10;
            this.Context.UiElements.VScrollBar.Invalidate();
            #endregion
        }
    }
}
