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
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace Demoder.PlanetMapViewer.DataClasses
{
    [Serializable]
    public struct SimpleColor
    {
        private int red { get; set; }
        private int green { get; set; }
        private int blue { get; set; }
        private int alpha { get; set; }
        [XmlAttribute("value")]
        public string Value
        {
            get
            {
                return String.Format("{0}:{1}:{2}:{3}", this.red, this.green, this.blue, this.alpha);
            }
            set
            {
                var values = value.Split(':');
                this.red = int.Parse(values[0]);
                this.green = int.Parse(values[1]);
                this.blue = int.Parse(values[2]);
                this.alpha = int.Parse(values[3]);
            }
        }

        public static implicit operator Color(SimpleColor color)
        {
            return new Color(color.red, color.green, color.blue, color.alpha);
        }
        public static implicit operator SimpleColor(Color color)
        {
            return new SimpleColor { red = color.R, green = color.G, blue = color.B, alpha = color.A };
        }
    }
}
