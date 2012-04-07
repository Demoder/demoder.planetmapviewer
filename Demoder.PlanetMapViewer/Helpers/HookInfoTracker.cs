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

namespace Demoder.PlanetMapViewer.Helpers
{
    public class HookInfoTracker
    {
        private Provider aoHookProvider;
        internal Context Context { get; private set; }
        private Dictionary<int, uint> ProcessCharacterMap = new Dictionary<int, uint>();


        public HookInfoTracker(Context context)
        {
            this.Context = context;
            this.aoHookProvider = new Provider();
            this.aoHookProvider.CharacterPositionEvent += HandleCharacterPositionEvent;
            this.aoHookProvider.HookStateChangeEvent += HandleHookStateChangeEvent;
            this.aoHookProvider.DynelNameEvent += HandleDynelNameEvent;
            this.aoHookProvider.QuestLocationEvent += HandleQuestLocationEvent;
            this.aoHookProvider.HookAo();
        }

        void HandleQuestLocationEvent(Provider sender, AoHook.Events.QuestLocationEventArgs e)
        {
            /*
            Vector2 cameraPosition = Vector2.Zero;
            lock (this.Processes)
            {
                var info = this.Processes[e.ProcessID];
                var quest = info.Mission;
                quest.ID = e.QuestID;
                quest.WorldPosition = new Vector3(e.WorldPosX, e.WorldPosY, e.WorldPosZ);
                quest.Zone.ID = e.ZoneID;
                quest.ZonePosition = new Vector3(e.ZonePosX, e.ZonePosY, e.ZonePosZ);
                info.LastModified.Restart();

                // Center camera on mission coord.
                if (quest.ZonePosition != default(Vector3))
                {
                    this.Context.State.CameraControl = CameraControl.Manual;
                    cameraPosition = this.Context.MapManager.GetPosition(quest.Zone.ID, quest.ZonePosition.X, quest.ZonePosition.Z);
                }
            }

            if (cameraPosition != Vector2.Zero)
            {
                this.Context.Camera.CenterOnPixel(cameraPosition.X, cameraPosition.Y);
            }
             */
        }

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

            lock (this.Context.State.PlayerInfo)
            {
                if (!this.Context.State.PlayerInfo.ContainsKey(e.DynelID))
                {
                    this.Context.State.PlayerInfo[e.DynelID]= new PlayerInfo();
                }
                
                var info = this.Context.State.PlayerInfo[e.DynelID];

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
                this.Context.UiElements.CharacterTrackerControl.UpdateCharacterList();
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
                this.Context.State.PlayerInfo[this.ProcessCharacterMap[e.ProcessID]].IsHooked = false;
                this.ProcessCharacterMap.Remove(e.ProcessID);

                this.Context.UiElements.CharacterTrackerControl.UpdateCharacterList();
            }
        }

        private void HandleCharacterPositionEvent(Provider sender, Demoder.AoHook.Events.CharacterPositionEventArgs e)
        {
            // Ignore anything which isn't player characters.
            if (e.DynelType != 50000) { return; }

            // Don't update if we have invalid data (such as while zoning)
            if (e.ZoneID == 0) { return; }

            var update = false;
           lock (this.Context.State.PlayerInfo)
           {
                var info = this.Context.State.PlayerInfo[e.DynelID];
                info.Position = new Vector3(e.X, e.Y, e.Z);
                info.Zone.ID = e.ZoneID;
                info.Zone.Name = e.ZoneName;
                if (info.InShadowlands != e.InShadowlands) { update = true; }
                info.InShadowlands = e.InShadowlands;
            }
           if (update)
           {
               this.Context.UiElements.CharacterTrackerControl.UpdateCharacterList();
           }
        }
    }
}
