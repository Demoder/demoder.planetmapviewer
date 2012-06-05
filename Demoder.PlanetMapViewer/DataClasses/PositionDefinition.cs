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
using Microsoft.Xna.Framework;

namespace Demoder.PlanetMapViewer.DataClasses
{
    public class PositionDefinition : ICloneable
    {
        /// <summary>
        /// If set to DrawMode.ViewPort, zone will be ignored and item will be placed relative to top left corner of viewport.
        /// </summary>
        public DrawMode Type = DrawMode.World;
        public uint Zone = 0;
        public int X = 0;
        public int Y = 0;

        public PositionDefinition() { }
        public PositionDefinition(float x, float y) : this((int)x, (int)y) { }
        public PositionDefinition(int x, int y)
        {
            this.Type = DrawMode.ViewPort;
            this.X = x;
            this.Y = y;
        }
        public PositionDefinition(uint zone, float x, float y) : this(zone, (int)x, (int)y) { }
        public PositionDefinition(uint zone, int x, int y)
            : this(x, y)
        {
            this.Type = DrawMode.World;
            this.Zone = zone;
        }

        public object Clone()
        {
            return new PositionDefinition
            {
                Type = this.Type,
                Zone = this.Zone,
                X = this.X,
                Y = this.Y
            };
        }

        public Vector2 GetPosition()
        {
            if (this.Zone == 0)
            {
                return new Vector2(this.X, this.Y);
            }

            return API.MapManager.GetPosition(this.Zone, this.X, this.Y);
        }
    }
}
