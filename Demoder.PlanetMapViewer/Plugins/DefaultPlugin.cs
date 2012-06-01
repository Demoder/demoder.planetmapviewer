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
using Demoder.PlanetMapViewer.PmvApi;
using Demoder.PlanetMapViewer.DataClasses;
using Microsoft.Xna.Framework;
using Demoder.AoHook;

namespace Demoder.PlanetMapViewer.Plugins
{
    public class DefaultPlugin : IPlugin
    {
        private Dictionary<int, uint> ProcessCharacterMap = new Dictionary<int, uint>();

        public DefaultPlugin()
        {
            Context.AoHookProvider.CharacterPositionEvent += HandleCharacterPositionEvent;
            Context.AoHookProvider.HookStateChangeEvent += HandleHookStateChangeEvent;
            Context.AoHookProvider.DynelNameEvent += HandleDynelNameEvent;
            Context.AoHookProvider.QuestLocationEvent += HandleQuestLocationEvent;
        }

        #region Retrieve IMapItems
        
        public CustomMapOverlay GetCustomOverlay()
        {
            var overlay = new CustomMapOverlay();
            overlay.MapItems.AddRange(this.GetCharacterLocators());
            overlay.MapItems.AddRange(this.GetMissionLocators());
            return overlay;
        }

        private IMapItem[] GetCharacterLocators()
        {
            var mapItems = new List<IMapItem>();
            var mapTexts = new List<IMapItem>();
            if (Context.State.PlayerInfo.Count == 0)
            {
                return new MapTexture[0];
            }

            var characters = Context.State.PlayerInfo.Values;


            foreach (var character in characters)
            {
                if (character == null || character.Zone == null || character.Position == null || character.Name == null) { continue; }
                var charLoc = new MapTexture()
                {
                    Texture = Context.Content.Textures.CharacterLocator,
                    KeyColor = Color.Yellow,
                    Position = new PositionDefinition(character.Zone.ID, character.Position.X, character.Position.Z),
                    PositionAlignment = MapItemAlignment.Center,
                };

                if (!character.IsHooked)
                {
                    charLoc.KeyColor = Color.Gray;
                }

                var charRealPos = charLoc.Position.GetPosition();

                var txt = new MapText
                {
                    Position = new PositionDefinition
                    {
                        Type = DrawMode.World,
                        X = (int)charRealPos.X,
                        Y = (int)charRealPos.Y + (int)charLoc.Size.Y / 2 + 5
                    },
                    Text = character.Name,
                    TextColor = Color.Yellow,
                    ShadowColor = Color.Black,
                    Shadow = true,
                    Font = FontType.MapCharLocator
                };
                if (!character.IsHooked)
                {
                    txt.TextColor = Color.LightGray;
                    txt.ShadowColor = Color.Gray;
                }

                mapTexts.Add(txt);

                mapItems.Add(charLoc);
            }

            mapItems.AddRange(mapTexts);
            return mapItems.ToArray();
        }

        private IMapItem[] GetMissionLocators()
        {
            return new MapTexture[0];
            /*
            var mapItems = new List<IMapItem>();
            var mapTexts = new List<IMapItem>();
            if (this.Context.HookInfo == null || this.Context.HookInfo.Processes == null)
            {
                return new MapTexture[0];
            }

            lock (this.Context.HookInfo.Processes)
            {
                foreach (var info in this.Context.HookInfo.Processes.Values)
                {
                    // We need both mission locator info and character name info
                    if (info == null) { continue; }
                    if (info.Mission == null || info.Mission.Zone == null || info.Mission.ZonePosition == null || info.Mission.ZonePosition == default(Vector3)) { continue; }
                    if (info.Character == null || info.Character.Name == null) { continue; }

                    var mapItem = new MapTexture();
                    mapItem.Texture = this.Context.Content.Textures.MissionLocator;
                    mapItem.Position = this.Context.MapManager.GetPosition(info.Mission.Zone.ID, info.Mission.ZonePosition.X, info.Mission.ZonePosition.Z);
                    
                    mapItem.PositionAlignment = MapItemAlignment.Bottom | MapItemAlignment.Center;
                    mapTexts.Add(new MapText
                    {
                        Position = new Vector2(mapItem.Position.X, mapItem.Position.Y + (int)mapItem.Texture.Height / 2 + 5),
                        Text = info.Character.Name,
                        TextColor = Microsoft.Xna.Framework.Color.White,
                        ShadowColor = Microsoft.Xna.Framework.Color.Black,
                        Shadow = true,
                        Font = this.Context.Content.Fonts.CharacterName
                    });

                    mapItems.Add(mapItem);
                }
            }
            mapItems.AddRange(mapTexts);
            return mapItems.ToArray();
             * */
        }
        #endregion

        #region AO Hook Provider
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

            lock (Context.State.PlayerInfo)
            {
                if (!Context.State.PlayerInfo.ContainsKey(e.DynelID))
                {
                    Context.State.PlayerInfo[e.DynelID] = new PlayerInfo();
                }

                var info = Context.State.PlayerInfo[e.DynelID];

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
                Context.UiElements.CharacterTrackerControl.UpdateCharacterList();
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
                Context.State.PlayerInfo[this.ProcessCharacterMap[e.ProcessID]].IsHooked = false;
                this.ProcessCharacterMap.Remove(e.ProcessID);

                Context.UiElements.CharacterTrackerControl.UpdateCharacterList();
            }
        }

        private void HandleCharacterPositionEvent(Provider sender, Demoder.AoHook.Events.CharacterPositionEventArgs e)
        {
            // Ignore anything which isn't player characters.
            if (e.DynelType != 50000) { return; }

            // Don't update if we have invalid data (such as while zoning)
            if (e.ZoneID == 0) { return; }

            var update = false;
            lock (Context.State.PlayerInfo)
            {
                var info = Context.State.PlayerInfo[e.DynelID];
                info.Position = new Vector3(e.X, e.Y, e.Z);
                info.Zone.ID = e.ZoneID;
                info.Zone.Name = e.ZoneName;
                if (info.InShadowlands != e.InShadowlands) { update = true; }
                info.InShadowlands = e.InShadowlands;
            }
            if (update)
            {
                Context.UiElements.CharacterTrackerControl.UpdateCharacterList();
            }
        }
        #endregion


    }
}
