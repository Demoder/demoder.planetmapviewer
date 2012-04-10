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
    /// Stores information about text and its position on the map
    /// </summary>
    public class MapText : IMapItem
    {
        #region IMapItem
        [XmlIgnore]
        internal Context Context { get; set; }
        [XmlIgnore]
        public MapItemType Type { get { return MapItemType.SpriteFont; } }
        [XmlAttribute("position")]
        public PositionDefinition Position {get; set;}
        [XmlAttribute("positionAlignment")]
        public MapItemAlignment PositionAlignment { get; set; }
        [XmlIgnore]
        public Vector2 Size
        {
            get
            {
                return this.Context.Content.Fonts.GetFont(this.Font).MeasureString(this.Text);
            }
        }

        [XmlText]
        public string Text;

        [XmlAttribute("shadowColor")]
        public Color ShadowColor = Color.Black;
        [XmlAttribute("textColor")]
        public Color TextColor = Color.White;
        [XmlAttribute("font")]
        public FontType Font = default(FontType);
        [XmlAttribute("haveShadow")]
        public bool Shadow = true;

        #endregion

        #region Constructors
        public MapText()
        {
            this.Position = new PositionDefinition();
            this.PositionAlignment = default(MapItemAlignment);
        }

        public MapText(Context context) : this()
        {
            this.Context = context;
        }
        #endregion

        public object Clone()
        {
            return new MapText(this.Context)
            {
                Font = this.Font,
                Position = (PositionDefinition)this.Position.Clone(),
                Shadow = this.Shadow,
                ShadowColor = this.ShadowColor,
                Text = (string)this.Text.Clone(),
                PositionAlignment = this.PositionAlignment,
                TextColor = this.TextColor
            };
        }
    }
}
