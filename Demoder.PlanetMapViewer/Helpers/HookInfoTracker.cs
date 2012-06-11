﻿/*
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
using Demoder.Common.AO;

namespace Demoder.PlanetMapViewer.Helpers
{
    public class HookInfoTracker : IDisposable
    {
        private Dictionary<int, uint> ProcessCharacterMap = new Dictionary<int, uint>();

        public Provider Provider { get; internal set; }

        public event Action TrackedDimensionChanged;

        public HookInfoTracker()
        {
            this.Provider = new Provider();
            this.Provider.CharacterPositionEvent += HandleCharacterPositionEvent;
            this.Provider.HookStateChangeEvent += HandleHookStateChangeEvent;
            this.Provider.DynelNameEvent += HandleDynelNameEvent;
        }

        #region AO Hook Provider
        // Todo: Move this to the general 
        private void HandleDynelNameEvent(Provider sender, Demoder.AoHook.Events.DynelNameEventArgs e)
        {
            if (!e.IsSelf) { return; }
            bool changed = false;

            lock (this.ProcessCharacterMap)
            {
                if (!this.ProcessCharacterMap.ContainsKey(e.ProcessID))
                {
                    this.ProcessCharacterMap[e.ProcessID] = e.DynelID;
                }
            }

            lock (API.State.PlayerInfo)
            {
                if (!API.State.PlayerInfo.ContainsKey(e.DynelID))
                {
                    API.State.PlayerInfo[e.DynelID] = new PlayerInfo();
                }

                var info = API.State.PlayerInfo[e.DynelID];

                if (e.DynelID != uint.MinValue && e.DynelID != uint.MaxValue)
                {
                    if (info.ID != e.DynelID) { changed = true; }
                    info.ID = e.DynelID;
                }
                if (e.DynelName != "NoName")
                {
                    if (e.DynelName != info.Name) { changed = true; }
                    info.Name = e.DynelName;
                }
            }
            if (changed)
            {
                API.UiElements.CharacterTrackerControl.UpdateCharacterList();
            }
        }


        private void HandleHookStateChangeEvent(Provider sender, Demoder.AoHook.Events.HookStateChangeEventArgs e)
        {
            lock (this.ProcessCharacterMap)
            {
                // We don't have anything to do if we're hooking.
                if (e.IsHooked) { return; }
                // We can't do anything if we don't know about the process
                if (!this.ProcessCharacterMap.ContainsKey(e.ProcessID)) { return; }

                // Update status of whether or not a player character is hooked.
                API.State.PlayerInfo[this.ProcessCharacterMap[e.ProcessID]].IsHooked = false;
                this.ProcessCharacterMap.Remove(e.ProcessID);

                API.UiElements.CharacterTrackerControl.UpdateCharacterList();
            }
        }

        private void HandleCharacterPositionEvent(Provider sender, Demoder.AoHook.Events.CharacterPositionEventArgs e)
        {
            // Ignore anything which isn't player characters.
            if (e.DynelType != 50000) { return; }

            // Don't update if we have invalid data (such as while zoning)
            if (e.ZoneID == 0) { return; }

            var update = false;
            lock (API.State.PlayerInfo)
            {
                var info = API.State.PlayerInfo[e.DynelID];
                info.Position = new Vector3(e.X, e.Y, e.Z);
                info.Zone.ID = e.ZoneID;
                info.Zone.Name = e.ZoneName;
                if (info.InShadowlands != e.InShadowlands) { update = true; }
                info.InShadowlands = e.InShadowlands;
                if (info.ServerID != e.ServerID) 
                {
                    info.ServerID = e.ServerID;
                    update = true;
                    this.UpdateTrackedDimension();
                }
            }
            if (update)
            {
                API.UiElements.CharacterTrackerControl.UpdateCharacterList();
            }
        }

        internal void UpdateTrackedDimension()
        {
            lock (API.State.PlayerInfo)
            {
                var dict = new Dictionary<Dimension, int>();
                foreach (var val in (Dimension[])Enum.GetValues(typeof(Dimension)))
                {
                    dict[val] = 0;
                }
                var dimensions = from pi in API.State.PlayerInfo
                                 where pi.Value.IsHooked
                                 where pi.Value.IsTrackedByCamera
                                 select pi.Value.Dimension;

                if (dimensions.Count() == 0)
                {
                    dimensions = from pi in API.State.PlayerInfo
                                 where pi.Value.IsHooked
                                 select pi.Value.Dimension;
                }
                foreach (var dim in dimensions)
                {
                    dict[dim]++;
                }
                var newDim = dict.OrderByDescending(k => k.Value).First().Key;
                if (newDim != API.State.CurrentDimension)
                {
                    if (this.TrackedDimensionChanged != null)
                    {
                        this.TrackedDimensionChanged();
                    }
                }
                API.State.CurrentDimension = newDim;
            }
        }
        #endregion

        public void Dispose()
        {
            this.Provider.CharacterPositionEvent -= HandleCharacterPositionEvent;
            this.Provider.HookStateChangeEvent -= HandleHookStateChangeEvent;
            this.Provider.DynelNameEvent -= HandleDynelNameEvent;
        }
    }
}
