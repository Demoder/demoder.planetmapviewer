/*
* Demoder.PlanetMapViewer
* Copyright (C) 2012, 2013 Demoder (demoder@demoder.me)
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
using Demoder.PlanetMapViewer.DataClasses;
using Demoder.PlanetMapViewer.Helpers;
using System.Drawing;

namespace Demoder.PlanetMapViewer.PmvApi
{
    public class GuiOverlay : MapOverlay
    {
        public GuiOverlay()
            : base()
        {
            this.BorderSize = 2;
        }

        public MapTexture DimmerTexture { get; private set; }
        public int BorderSize { get; set; }

        internal void GenerateDimmerTexture()
        {
            if (this.DimmerTexture != null) { return; }

            // Get top left corner.
            var topLeft = new Point(
                this.MapItems.Min(i => i.Position.X),
                this.MapItems.Min(i => i.Position.Y)
                );

            // Get bottom right corner by adding position on screen + size, then subtracting the top left corners position from that.
            var bottomRight = new Point(
                (int)this.MapItems.Max(i => FrameDrawer.GetRealPosition(i).X + i.Size.X),
                (int)this.MapItems.Max(i => FrameDrawer.GetRealPosition(i).Y + i.Size.Y)
                );

            // Subtract from top left corner
            topLeft.X -= this.BorderSize;
            topLeft.Y -= this.BorderSize;

            // Add border size * 2 to width & height 
            //  Once to compensate for the extra width/height from the shifted top left corner; 
            //  Once for shifting bottom right corner by borderSize
            bottomRight.X += this.BorderSize;
            bottomRight.Y += this.BorderSize;

            // Generate the texture definition.
            this.DimmerTexture = new MapTexture
            {
                KeyColor = Microsoft.Xna.Framework.Color.White,
                Position = new PositionDefinition(
                    topLeft.X, 
                    topLeft.Y),
                PositionAlignment = MapItemAlignment.Top | MapItemAlignment.Left,
                Size = new Microsoft.Xna.Framework.Vector2(
                    bottomRight.X - topLeft.X, 
                    bottomRight.Y - topLeft.Y),
                Texture = API.Content.Textures.GuiBackgroundDimmer50
            };
        }
    }
}
