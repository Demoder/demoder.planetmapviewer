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
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Demoder.Common.AO;

namespace Demoder.PlanetMapViewer.DataClasses
{
    public class PlayerInfo
    {
        public PlayerInfoKey Identity = new PlayerInfoKey(0, 0);
        public uint ID { 
            get { return this.Identity.CharacterID; }
            set { this.Identity.CharacterID = value; }
        }
        public string Name;
        public ZoneInfo Zone = new ZoneInfo();
        public Vector3 Position = new Vector3();
        public bool InShadowlands = false;

        /// <summary>
        /// Is this character tracked by camera?
        /// </summary>
        public bool IsTrackedByCamera = false;

        /// <summary>
        /// Is this entrys parent still hooked?
        /// </summary>
        public bool IsHooked = true;

        public uint ServerID = 0;

        public Dimension Dimension
        {
            get
            {
                return PlayerInfoKey.GetDimension(this.ServerID);
            }
        }
    }
}