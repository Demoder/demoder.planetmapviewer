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
using Demoder.Common.Serialization;
using Demoder.Common.AO.Towerwars.DataClasses;
using Demoder.Common.Web;
using Demoder.PlanetMapViewer.DataClasses;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using Demoder.Common.AO;
using Demoder.Common.Attributes;

namespace Demoder.PlanetMapViewer.Plugins
{
    [Plugin("TowerStatus", 120)]
    [Description("Displays current tower war status on the planet map")]
    public class TowerStatusPlugin : IPlugin
    {
        #region Settings
        [Setting(Dimension.Rimor)]
        public Dimension Dimension { get; set; }

        [Setting(true)]
        public bool ShowNames { get; set; }
        #endregion

        private string source="http://towerwars.info/m/TowerDistribution.php";

        public TowerStatusPlugin()
        {
            this.Dimension = Dimension.Rimor;
            API.MapManager.MapChangedEvent += new Action(this.HandleMapChangedEvent);
        }

        public void Dispose()
        {
            API.MapManager.MapChangedEvent -= new Action(this.HandleMapChangedEvent);
        }

        private void HandleMapChangedEvent()
        {
            API.PluginManager.ChangePluginStatus<TowerStatusPlugin>(API.MapManager.CurrentMap.Type == MapType.Rubika);
        }   

        public CustomMapOverlay GetCustomOverlay()
        {
            // Get tower data.
            var qb = new UriQueryBuilder();
            {
                dynamic q=qb;
                q.d = (int)this.Dimension;
                q.output="xml";
                q.limit=300;
                q.minlevel=1;
                q.maxlevel=300;
            }

            var towerData = Xml.Deserialize<TowerSites>(qb.ToUri(new Uri(this.source)));
            if (towerData == null) { return null; }

            var overlay = new CustomMapOverlay(towerData.Sites.Count);
           
            foreach (var site in towerData.Sites)
            {
                var tex = new MapTexture
                {
                    Position = new PositionDefinition(site.ZoneID, site.CenterX, site.CenterY),
                    Texture = new TextureDefinition(TextureType.Content, "TowerTexture"),
                    Size = new Vector2(8, 8),
                    PositionAlignment = MapItemAlignment.Center | MapItemAlignment.Bottom,
                };

                switch (site.Faction)
                {
                    case Common.AO.Faction.Neutral:
                        //tex.KeyColor = Color.White;
                        tex.KeyColor = Color.MediumAquamarine;
                        break;
                    case Common.AO.Faction.Clan:
                        tex.KeyColor = Color.DarkOrange;
                        break;
                    case Common.AO.Faction.Omni:
                        //tex.KeyColor = Color.DeepSkyBlue;
                        tex.KeyColor = Color.SkyBlue;
                        break;
                    default:
                        tex.KeyColor = Color.Purple;
                        break;
                }

                MapText text=null;
                if (this.ShowNames)
                {
                    text = new MapText
                    {
                        Font = FontType.GuiSmall,
                        Shadow = true,
                        ShadowColor = Color.Black,
                        TextColor = tex.KeyColor,
                        Text = site.Guild
                    };
                }

                overlay.MapItems.Add(new MapTextureText(tex, text) { PositionAlignment = MapItemAlignment.Bottom });
            }

            return overlay;
        }       
    }
}
