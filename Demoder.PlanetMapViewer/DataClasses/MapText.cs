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

namespace Demoder.PlanetMapViewer.DataClasses
{
    /// <summary>
    /// Stores information about text and its position on the map
    /// </summary>
    public class MapText : IMapItem
    {
        #region IMapItem
        private Context context;
        public MapItemType Type { get { return MapItemType.SpriteFont; } }
        public MapItemAlignment PositionAlignment { get; set; }
        public Vector2 Position {get; set;}
        public Vector2 Size
        {
            get
            {
                return this.context.Content.Fonts.GetFont(this.Font).MeasureString(this.Text);
            }
        }
        #endregion

        public MapText(Context context)
        {
            this.context = context;
            this.Position = default(Vector2);
            this.PositionAlignment = default(MapItemAlignment);
        }

        public string Text;
        public Color ShadowColor = Color.Black;
        public Color TextColor = Color.White;
        public FontType Font = default(FontType);
        public bool Shadow = true;

        public bool IsRelativePosition
        {
            get { throw new NotImplementedException(); }
        }




        public object Clone()
        {
            return new MapText(this.context)
            {
                Font = this.Font,
                Position = this.Position,
                Shadow = this.Shadow,
                ShadowColor = this.ShadowColor,
                Text = (string)this.Text.Clone(),
                PositionAlignment = this.PositionAlignment,
                TextColor = this.TextColor
            };
        }
    }
}
