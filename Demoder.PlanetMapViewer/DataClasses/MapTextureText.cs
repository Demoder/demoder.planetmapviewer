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
using Demoder.PlanetMapViewer.Helpers;

namespace Demoder.PlanetMapViewer.DataClasses
{
    public class MapTextureText : IMapItem
    {
        private MapTexture texture;
        private MapText text;
        private int textSpacing;

        public MapTextureText(MapTexture texture, MapText text, int textSpacing = 2)
        {
            this.texture = texture;
            this.text = text;
            this.textSpacing = textSpacing;
            this.AutoTextAlignment = true;
        }

        public MapItemType Type { get { return MapItemType.TextureWithAttachedSpriteFont; } }

        /// <summary>
        /// Where is text located, relative to texture?
        /// </summary>
        public MapItemAlignment PositionAlignment { get; set; }

        /// <summary>
        /// Whether to override MapText' alignment setting based on defined PositionAlignment.<br/>
        /// Defaults to true.<br/>
        /// Generally, you shouldn't change this value.
        /// </summary>
        public bool AutoTextAlignment { get; set; }
        
        public PositionDefinition Position
        {
            get { return this.texture.Position; }
        }

        public Microsoft.Xna.Framework.Vector2 Size
        {
            get { return this.texture.Size; }
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public MapTextureText Clone()
        {
            return new MapTextureText(this.texture, this.text, this.textSpacing)
            {
                PositionAlignment = this.PositionAlignment
            };
        }

        public IMapItem[] ToMapItems()
        {
            var items = new IMapItem[2];
            items[0] = this.texture;

            if (this.text == null)
            {
                items[1] = null;
                return items;
            }

            var text = this.text.Clone() as MapText;
            if (this.AutoTextAlignment)
            {
                text.PositionAlignment = MapItemAlignment.Center;
            }
            var texturePos = FrameDrawer.GetRealPosition(this.texture);
            // Adjust vertical stuff.
            if (this.PositionAlignment.HasFlag(MapItemAlignment.Bottom))
            {
                if (this.AutoTextAlignment)
                {
                    text.PositionAlignment = MapItemAlignment.Top;
                }
                text.Position.Y = (int)(texturePos.Y + texture.Size.Y + this.textSpacing);
            }

            else if (this.PositionAlignment.HasFlag(MapItemAlignment.Top))
            {
                if (this.AutoTextAlignment)
                {
                    text.PositionAlignment = MapItemAlignment.Bottom;
                }
                text.Position.Y = (int)(texturePos.Y - this.textSpacing);
            }
            else 
            {
                text.Position.Y = (int)(texturePos.Y + (texture.Size.Y / 2));
            }

            // Adjust horizontal stuff
            if (this.PositionAlignment.HasFlag(MapItemAlignment.Left))
            {
                if (this.AutoTextAlignment)
                {
                    text.PositionAlignment |= MapItemAlignment.Right;
                }
                text.Position.X = (int)(texturePos.X - this.textSpacing);
            }
            else if (this.PositionAlignment.HasFlag(MapItemAlignment.Right))
            {
                if (this.AutoTextAlignment)
                {
                    text.PositionAlignment |= MapItemAlignment.Left;
                }
                text.Position.X = (int)(texturePos.X + texture.Size.X + textSpacing);
            }
            else
            {
                text.Position.X = (int)(texturePos.X + (texture.Size.X / 2));
            }
            items[1] = text;

            return items;
        }
    }
}
