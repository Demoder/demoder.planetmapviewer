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
using Demoder.Common.AO;

namespace Demoder.PlanetMapViewer.DataClasses
{
    public class ContextState
    {
        /// <summary>
        /// Which map type is currently selected, if any?
        /// </summary>
        public MapType MapType = MapType.Rubika;
        /// <summary>
        /// Is autoswitching between map types enabled?
        /// </summary>
        public bool MapTypeAutoSwitching = true;
        public CameraControl CameraControl = CameraControl.ActiveCharacter;
        /// <summary>
        /// Current window mode
        /// </summary>
        public WindowMode WindowMode = WindowMode.Normal;

        /// <summary>
        /// Percent magnification. 2 = twice size, 0.5=half size.
        /// </summary>
        public float Magnification = 1;

        public Dictionary<PlayerInfoKey, PlayerInfo> PlayerInfo = new Dictionary<PlayerInfoKey, PlayerInfo>();

        public Dimension CurrentDimension {get;internal set;}
    }
}
