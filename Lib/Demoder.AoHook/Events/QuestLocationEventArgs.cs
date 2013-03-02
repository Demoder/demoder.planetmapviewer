/*
* Demoder.AoHook
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
using System.Text;

namespace Demoder.AoHook.Events
{
    [Serializable]
    public class QuestLocationEventArgs : HookEventArgs
    {
        public uint QuestID { get; private set; }
        public uint ZoneID { get; private set; }
        
        public float WorldPosX { get; private set; }
        public float WorldPosY { get; private set; }
        public float WorldPosZ { get; private set; }

        public float ZonePosX { get; private set; }
        public float ZonePosY { get; private set; }
        public float ZonePosZ { get; private set; }
        
        public QuestLocationEventArgs(AoHookBridge.Events.QuestLocationEventArgs e)
            : base(e.ProcessId, HookEventType.QuestLocation, e.Time)
        {
            this.QuestID = e.QuestID;
            this.ZoneID = e.ZoneID;

            this.WorldPosX = e.WorldPos.X;
            this.WorldPosY = e.WorldPos.Y;
            this.WorldPosZ = e.WorldPos.Z;

            this.ZonePosX = e.ZonePos.X;
            this.ZonePosY = e.ZonePos.Y;
            this.ZonePosZ = e.ZonePos.Z;
        }
    }
}
