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
using System.Threading;
using System.ComponentModel;
using Demoder.Common.Attributes;

namespace Demoder.PlanetMapViewer.Plugins
{
    [Plugin("Character Locator")]
    [Description("Displays character locators on the planet map")]
    public class CharacterLocatorPlugin : IPlugin
    {
        #region settings
        [SettingOption(LoadedFont.Silkscreen7)]
        public LoadedFont CharacterLocatorFont { get; set; }
        #endregion

        private Dictionary<int, uint> ProcessCharacterMap = new Dictionary<int, uint>();

        public CharacterLocatorPlugin()
        {
            API.AoHookProvider.CharacterPositionEvent += HandleCharacterPositionEvent;
            API.AoHookProvider.HookStateChangeEvent += HandleHookStateChangeEvent;
            API.AoHookProvider.DynelNameEvent += HandleDynelNameEvent;
        }

        #region Retrieve IMapItems
        
        public CustomMapOverlay GetCustomOverlay()
        {
            var overlay = new CustomMapOverlay
            {
                DrawOrder = int.MaxValue
            };

            overlay.MapItems.AddRange(this.GetCharacterLocators());
            return overlay;
        }

        private IMapItem[] GetCharacterLocators()
        {
            var mapTextureTexts = new List<IMapItem>();
            if (API.State.PlayerInfo.Count == 0)
            {
                return new MapTexture[0];
            }

            var characters = API.State.PlayerInfo.Values.ToArray();

            foreach (var character in characters)
            {
                if (character == null || character.Zone == null || character.Position == null || character.Name == null) { continue; }
                var texture = new MapTexture()
                {
                    Texture = API.Content.Textures.CharacterLocator,
                    KeyColor = Color.Yellow,
                    Position = new PositionDefinition(character.Zone.ID, character.Position.X, character.Position.Z),
                    PositionAlignment = MapItemAlignment.Center,
                };

                if (!character.IsHooked)
                {
                    texture.KeyColor = Color.Gray;
                }

                var charRealPos = texture.Position.GetPosition();

                var txt = new MapText
                {
                    Position = new PositionDefinition(),
                    PositionAlignment = MapItemAlignment.Center,
                    Text = character.Name,
                    TextColor = Color.Yellow,
                    ShadowColor = Color.Black,
                    Shadow = true,
                    Font = API.Content.Fonts.GetFont(this.CharacterLocatorFont)
                };

                if (!character.IsHooked)
                {
                    txt.TextColor = Color.LightGray;
                    txt.ShadowColor = Color.Gray;
                }

                mapTextureTexts.Add(new MapTextureText(texture, txt, 1)
                {
                    PositionAlignment = MapItemAlignment.Bottom | MapItemAlignment.Center,
                });
            }

            return mapTextureTexts.ToArray();
        }

        #endregion

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
            }
            if (update)
            {
                API.UiElements.CharacterTrackerControl.UpdateCharacterList();
            }
        }
        #endregion



        public void Dispose()
        {
            API.AoHookProvider.CharacterPositionEvent -= HandleCharacterPositionEvent;
            API.AoHookProvider.HookStateChangeEvent -= HandleHookStateChangeEvent;
            API.AoHookProvider.DynelNameEvent -= HandleDynelNameEvent;
        }
    }
}
