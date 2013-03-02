/*
* Demoder.PlanetMapViewer
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
using System.Linq;
using System.Text;
using Demoder.PlanetMapViewer.PmvApi;
using System.ComponentModel;
using Demoder.PlanetMapViewer.DataClasses;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Demoder.Common.Attributes;

namespace Demoder.PlanetMapViewer.Plugins
{
#if PMVNET
    [Plugin("PmvNetwork")]
    [Description("Share character locations with friends!")]
    public class PmvNetworkPlugn : IPlugin
    {
        #region Settings
        [Setting(LoadedFont.Silkscreen7)]
        public LoadedFont CharacterLocatorFont { get; set; }

        private Color locatorColor = Color.Red;

        #endregion

        private Dictionary<PlayerInfoKey, PmvNetCharacterInfo> characters = new Dictionary<PlayerInfoKey, PmvNetCharacterInfo>();
        private Task netTask;

        public PmvNetworkPlugn()
        {

            this.characters.Add(
                new PlayerInfoKey(3300, 123456),
                new PmvNetCharacterInfo
                {
                    Name = "PMVNet_Test",
                    Zone = 566,
                    X = 391,
                    Z = 316
                });
        }


        public IEnumerable<MapOverlay> GetCustomOverlay()
        {
            MapOverlay overlay = new MapOverlay();

            foreach (PmvNetCharacterInfo character in this.characters.Values.ToArray())
            {
                var texture = new MapTexture()
                {
                    Texture = API.Content.Textures.CharacterLocator,
                    KeyColor = this.locatorColor,
                    Position = new PositionDefinition(character.Zone, character.X, character.Z),
                    PositionAlignment = MapItemAlignment.Center,
                };

                var txt = new MapText
                {
                    Position = new PositionDefinition(),
                    PositionAlignment = MapItemAlignment.Center,
                    Text = character.Name,
                    TextColor = texture.KeyColor,
                    OutlineColor = Color.Black,
                    Outline = true,
                    Font = API.Content.Fonts.GetFont(this.CharacterLocatorFont)
                };

                var mpt = new MapTextureText(texture, txt, 1)
                {
                    PositionAlignment = MapItemAlignment.Bottom | MapItemAlignment.Center,
                };

                overlay.MapItems.Add(mpt);
            }            
            yield return overlay;
            yield break;
        }

        public void Dispose()
        {
            
        }
    }
    

    public class PmvNetCharacterInfo
    {
        [StreamData(0)]
        public Guid Source { get; set; }
        [StreamData(1)]
        public uint Zone { get; set; }
        [StreamData(2)]
        public uint X { get; set; }
        [StreamData(3)]
        public uint Z { get; set; }
        [StreamData(4)]
        public string Name { get; set; }

        public PmvNetCharacterInfo()
        {
            
        }
    }
#endif
}
