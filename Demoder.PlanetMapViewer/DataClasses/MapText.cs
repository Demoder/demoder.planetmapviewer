﻿/*
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
        public MapItemType Type { get { return MapItemType.SpriteFont; } }
        public PositionDefinition Position {get; set;}
        public MapItemAlignment PositionAlignment { get; set; }
        [XmlIgnore]
        public Vector2 Size
        {
            get
            {
                if (this.Font == null) { return Vector2.Zero; }
                return this.Font.MeasureString(this.Text);
            }
        }

        public string Text;

        public SimpleColor OutlineColor = Color.Black;
        public SimpleColor TextColor = Color.White;
        public SpriteFont Font = null;
        public bool Outline = true;

        #endregion

        #region Constructors
        public MapText()
        {
            this.Position = new PositionDefinition();
            this.PositionAlignment = default(MapItemAlignment);
        }
        #endregion

        public object Clone()
        {
            return new MapText
            {
                Font = this.Font,
                Position = (PositionDefinition)this.Position.Clone(),
                Outline = this.Outline,
                OutlineColor = this.OutlineColor,
                Text = this.Text!=null ? (string)this.Text.Clone() : null,
                PositionAlignment = this.PositionAlignment,
                TextColor = this.TextColor
            };
        }
    }
}
