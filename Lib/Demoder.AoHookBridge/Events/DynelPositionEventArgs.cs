/*
* Demoder.AoHookBridge
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
using System.Text;

namespace Demoder.AoHookBridge.Events
{
    [Serializable]
    public class DynelPositionEventArgs : BridgeEventArgs
    {
        public uint DynelType { get; private set; }
        public uint DynelID { get; private set; }
        public uint ZoneID { get; private set; }
        public string ZoneName { get; private set; }
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }

        public DynelPositionEventArgs(uint dynelType, uint dynelId, uint zoneId, string zoneName, float x, float y, float z)
            : base(BridgeEventType.CharacterPosition)
        {
            this.DynelType = dynelType;
            this.DynelID = dynelId;
            this.ZoneID = zoneId;
            this.ZoneName = zoneName;
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(obj, null))
            {
                return false;
            }
            if (Object.ReferenceEquals(obj, this))
            {
                return true;
            }

            if (obj is DynelPositionEventArgs)
            {
                var blah = obj as DynelPositionEventArgs;
                if (blah.ZoneID != this.ZoneID) { return false; }
                if (blah.ZoneName != this.ZoneName) { return false; }
                if (blah.X != this.X) { return false; }
                if (blah.Y != this.Y) { return false; }
                if (blah.Z != this.Z) { return false; }
                if (blah.DynelType != this.DynelType) { return false; }
                if (blah.DynelID != this.DynelID) { return false; }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
