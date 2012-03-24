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
using Demoder.AoHook;
using System.Collections.Concurrent;
using Demoder.PlanetMapViewer.DataClasses;
using Microsoft.Xna.Framework;
using System.Threading;

namespace Demoder.PlanetMapViewer
{
    public class HookInfoTracker
    {
        private Provider aoHookProvider;
        public Dictionary<int, AoInfo> Processes = new Dictionary<int, AoInfo>();


        public HookInfoTracker()
        {
            this.aoHookProvider = new Provider();
            this.aoHookProvider.CharacterPositionEvent += HandleCharacterPositionEvent;
            this.aoHookProvider.HookStateChangeEvent += HandleHookStateChangeEvent;
            this.aoHookProvider.DynelNameEvent += HandleDynelNameEvent;
            this.aoHookProvider.HookAo();
        }

        private void HandleDynelNameEvent(Provider sender, Demoder.AoHook.Events.DynelNameEventArgs e)
        {
            if (!e.IsSelf) { return; }
            lock (this.Processes)
            {
                var info = this.Processes[e.ProcessID];
                if (e.DynelID != uint.MinValue && e.DynelID != uint.MaxValue)
                {
                    info.Character.ID = e.DynelID;
                }
                if (e.DynelName != "NoName")
                {
                    info.Character.Name = e.DynelName;
                }
                info.LastModified.Restart();
            }
        }


        private void HandleHookStateChangeEvent(Provider sender, Demoder.AoHook.Events.HookStateChangeEventArgs e)
        {
            lock (this.Processes)
            {
                if (e.IsHooked)
                {
                    this.Processes[e.ProcessID] = new AoInfo(e.ProcessID);
                }
                else
                {
                    if (this.Processes.ContainsKey(e.ProcessID))
                    {
                        var info = this.Processes[e.ProcessID];
                        this.Processes.Remove(e.ProcessID);
                    }
                }
            }
        }

        private void HandleCharacterPositionEvent(Provider sender, Demoder.AoHook.Events.CharacterPositionEventArgs e)
        {
            // Don't update if we have invalid data (such as while zoning)
            if (e.ZoneID == 0) { return; }

            lock (this.Processes)
            {
                var info = this.Processes[e.ProcessID];
                info.Position = new Vector3(e.X, e.Y, e.Z);
                info.Zone.ID = e.ZoneID;
                info.Zone.Name = e.ZoneName;
                info.LastModified.Restart();
            }
        }
    }
}
