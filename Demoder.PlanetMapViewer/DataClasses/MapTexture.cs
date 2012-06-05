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
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;

namespace Demoder.PlanetMapViewer.DataClasses
{
    /// <summary>
    /// Stores information about a charcter locator, and associated text
    /// </summary>
    public class MapTexture : IMapItem
    {
        private Vector2 size;
        #region IMapItem
        public MapItemType Type { get { return MapItemType.Texture; } }
        /// <summary>
        /// Item position on the map
        /// </summary>
        public PositionDefinition Position { get; set; }
        /// <summary>
        /// Where on the texture is Position located?
        /// </summary>
        public MapItemAlignment PositionAlignment { get; set; }
        public TextureDefinition Texture { get; set; }
        public SimpleColor KeyColor = Color.White;

        public Vector2 Size
        {
            get
            {
                if (this.size != default(Vector2)) { return this.size; }
                if (this.Texture == null) { return default(Vector2); }
                var tex = API.Content.Textures.GetTexture(this.Texture);
                return new Vector2(tex.Width, tex.Height);
            }
            set
            {
                this.size = value;
            }
        }
        #endregion
        public MapTexture()
        {
            this.Position = new PositionDefinition();
            this.PositionAlignment = default(MapItemAlignment);
        }

        public object Clone()
        {
            var item = new MapTexture
            {
                KeyColor = this.KeyColor,
                Position = (PositionDefinition)this.Position.Clone(),
                PositionAlignment = this.PositionAlignment,
                Texture = this.Texture
            };
            if (this.size != default(Vector2))
            {
                item.size = this.size;
            }
            return item;
        }
    }
}
