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
using bEvents = Demoder.AoHookBridge.Events;

namespace Demoder.AoHook.Events
{
    [Serializable]
    public class DynelNameEventArgs : HookEventArgs
    {
        public uint DynelType { get; private set; }
        public uint ServerID {get; private set;}
        public uint DynelID { get; private set; }
        public string DynelName { get; private set; }
        /// <summary>
        /// Is it the character controlled by this instance?
        /// </summary>
        public bool IsSelf { get; private set; }

        public DynelNameEventArgs(bEvents.DynelNameEventArgs e)
            : base(e.ProcessId, HookEventType.DynelName, e.Time)
        {
            this.DynelType = e.DynelType;
            this.DynelID = e.DynelID;
            this.DynelName = e.DynelName;
            this.IsSelf = e.IsSelf;
            this.ServerID = e.ServerID;
        }
    }
}
