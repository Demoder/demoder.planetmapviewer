/*
* Demoder.AoHook
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

namespace Demoder.AoHook.Events
{
    public class CharacterPositionEventArgs : HookEventArgs
    {
        public uint DynelType { get; private set; }
        public uint DynelID { get; private set; }
        public uint ZoneID { get; private set; }
        public string ZoneName { get; private set; }
        public bool InShadowlands { get; private set; }
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }

        internal CharacterPositionEventArgs(int processID, uint dynelType, uint dynelId, uint zoneId, string zoneName, bool inShadowlands, float x, float y, float z, DateTime time = default(DateTime)) : base(processID, HookEventType.CharacterPosition, time)
        {
            this.DynelType = dynelType;
            this.DynelID = dynelId;
            this.ZoneID = zoneId;
            this.ZoneName = zoneName;
            this.InShadowlands = inShadowlands;
            this.X = x;
            this.Y = y;
            this.Z = z;
        }



        /// <summary>
        /// Retrieves the average absolute difference between this and the provided instance
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public float GetAvgDifference(CharacterPositionEventArgs target)
        {
            var diffX = Math.Abs(target.X - this.X);
            var diffY = Math.Abs(target.Y - this.Y);
            var diffZ = Math.Abs(target.Z - this.Z);

            return (diffX + diffY + diffZ) / 3;
        }
    }
}
