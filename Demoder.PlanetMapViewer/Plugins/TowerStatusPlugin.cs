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
using Demoder.PlanetMapViewer.Helpers;
using Demoder.Common.Extensions;

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

        [Setting(true)]
        public bool ShowRecentAttacks { get; set; }
        #endregion

        private const string twTowerDistribution="http://towerwars.info/m/TowerDistribution.php";
        private const string twHistorySearch = "http://towerwars.info/m/HistorySearch.php";

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
            var overlay = new CustomMapOverlay();
            this.GenerateTowerFieldInfo(ref overlay);
            this.GenerateRecentAttackInfo(ref overlay);
            return overlay;
        }

        private void GenerateRecentAttackInfo(ref CustomMapOverlay overlay)
        {
            if (!this.ShowRecentAttacks) { return; }
            // Get tower data.
            var qb = new UriQueryBuilder();
            {
                dynamic q = qb;
                q.d = (int)this.Dimension;
                q.output = "xml";
                q.limit = 5;
                q.sortorder = "desc";
                q.chopmethod = "last";
                q.starttime = Demoder.Common.Misc.Unixtime(DateTime.Now.Subtract(new TimeSpan(2, 0, 0)));
            }
            var towerData = Xml.Deserialize<TowerAttacks>(qb.ToUri(new Uri(twHistorySearch)));

            if (towerData == null)
            {
                overlay.MapItems.Add(new MapText
                {
                    Position = new PositionDefinition(5, 5),
                    PositionAlignment = MapItemAlignment.Top | MapItemAlignment.Left,
                    TextColor = Color.White,
                    ShadowColor = Color.Black,
                    Shadow = true,
                    Font = FontType.MapCharLocator,
                    Text = "Error fetching recent attacks for " + this.Dimension.ToString()
                });
                return;
            }
            if (towerData.Attacks.Count == 0) {
                overlay.MapItems.Add(new MapText
                {
                    Position = new PositionDefinition(5, 5),
                    PositionAlignment = MapItemAlignment.Top | MapItemAlignment.Left,
                    TextColor = Color.White,
                    ShadowColor = Color.Black,
                    Shadow = true,
                    Font = FontType.MapCharLocator,
                    Text = "No recent attacks on " + this.Dimension.ToString()
                });
                return; 
            }

            overlay.MapItems.Add(new MapText
            {
                Position = new PositionDefinition(5, 5),
                PositionAlignment = MapItemAlignment.Top | MapItemAlignment.Left,
                TextColor = Color.White,
                ShadowColor = Color.Black,
                Shadow = true,
                Font = FontType.MapCharLocator,
                Text = "Recent tower attacks on "+ this.Dimension.ToString()
            });

            foreach (var attack in towerData.Attacks)
            {
                var last = overlay.MapItems.Last();
                var y = last.Position.Y + last.Size.Y + 2;
                var x = 5;
                // Time
                overlay.MapItems.Add(new MapText
                {
                    Font = FontType.MapCharLocator,
                    Shadow = true,
                    ShadowColor = Color.Black,
                    TextColor = Color.White,
                    PositionAlignment = MapItemAlignment.Top | MapItemAlignment.Left,
                    Position = new PositionDefinition(x, y),
                    Text = "[" + attack.Time.ToShortTimeString() + "]"
                });

                x += 50;

                // Attacked site ZONE
                overlay.MapItems.Add(new MapText
                {
                    Font = FontType.MapCharLocator,
                    Shadow = true,
                    ShadowColor = Color.Black,
                    TextColor = this.GetFactionColor(attack.DefenderFaction),
                    PositionAlignment = MapItemAlignment.Top | MapItemAlignment.Left,
                    Position = new PositionDefinition(x, y),
                    Text = attack.ZoneShortName
                });
                x+=50;

                // Attacked site ID
                overlay.MapItems.Add(new MapText
                {
                    Font = FontType.MapCharLocator,
                    Shadow = true,
                    ShadowColor = Color.Black,
                    TextColor = this.GetFactionColor(attack.DefenderFaction),
                    PositionAlignment = MapItemAlignment.Top | MapItemAlignment.Left,
                    Position = new PositionDefinition(x, y),
                    Text = String.Format("x{0}", attack.SiteID)
                });
                x += 30;

                // Attacker nickname
                overlay.MapItems.Add(new MapText
                {
                    Font = FontType.MapCharLocator,
                    Shadow = true,
                    ShadowColor = Color.Black,
                    TextColor = this.GetFactionColor(attack.AttackerFaction),
                    PositionAlignment = MapItemAlignment.Top | MapItemAlignment.Left,
                    Position = new PositionDefinition(x, y),
                    Text = attack.AttackerNickName
                });
                x += 75;

                if (!String.IsNullOrWhiteSpace(attack.AttackerGuildName))
                {
                    overlay.MapItems.Add(new MapText
                    {
                        Font = FontType.MapCharLocator,
                        Shadow = true,
                        ShadowColor = Color.Black,
                        TextColor = Color.White,
                        PositionAlignment = MapItemAlignment.Top | MapItemAlignment.Left,
                        Position = new PositionDefinition(x, y),
                        Text = "of"
                    });

                    x += 20;

                    overlay.MapItems.Add(new MapText
                    {
                        Font = FontType.MapCharLocator,
                        Shadow = true,
                        ShadowColor = Color.Black,
                        TextColor = this.GetFactionColor(attack.AttackerFaction),
                        PositionAlignment = MapItemAlignment.Top | MapItemAlignment.Left,
                        Position = new PositionDefinition(x, y),
                        Text = attack.AttackerGuildName
                    });
                }


            }

            /*
            var sb = new MapTextBuilder(FontType.MapCharLocator, Color.White, Color.Black, true, MapItemAlignment.Left | MapItemAlignment.Top);
            sb.Text("Recent tower attacks:").Break();
            foreach (var attack in towerData.Attacks)
            {
                

                // Time of attack
                sb.Text("["+attack.Time.ToShortTimeString()+"]  ");
                // Site being attacked
                sb.Text(String.Format("{0,4}  {1,3} ", attack.ZoneShortName, String.Format("x{0}",attack.SiteID)), textColor: this.GetFactionColor(attack.DefenderFaction));
                // Attacker
                sb.Text(String.Format("{0,14}", attack.AttackerNickName), textColor: this.GetFactionColor(attack.AttackerFaction));
                if (!String.IsNullOrWhiteSpace(attack.AttackerGuildName))
                {
                    sb.Text(" of ");
                    sb.Text(attack.AttackerGuildName, textColor: this.GetFactionColor(attack.AttackerFaction));
                }
                sb.Break();
            }
            
            overlay.MapItems.AddRange(sb.ToMapItem(DrawMode.ViewPort, 5, 5));
            */
        }

        private Color GetFactionColor(Faction faction)
        {
            switch (faction)
            {
                case Faction.Neutral:
                    return Color.White;
                case Faction.Clan:
                    return Color.Orange;
                case Faction.Omni:
                    return Color.DeepSkyBlue;
                default:
                case Faction.Unknown:
                    return Color.Pink;
            }
        }

        private void GenerateTowerFieldInfo(ref CustomMapOverlay overlay)
        {
            // Get tower data.
            var qb = new UriQueryBuilder();
            {
                dynamic q = qb;
                q.d = (int)this.Dimension;
                q.output = "xml";
                q.limit = 300;
                q.minlevel = 1;
                q.maxlevel = 300;
            }

            var towerData = Xml.Deserialize<TowerSites>(qb.ToUri(new Uri(twTowerDistribution)));
            if (towerData == null) { return; }
            foreach (var site in towerData.Sites)
            {
                var tex = new MapTexture
                {
                    Position = new PositionDefinition(site.ZoneID, site.CenterX, site.CenterY),
                    Texture = new TextureDefinition(TextureType.Content, "TowerTexture"),
                    Size = new Vector2(8, 8),
                    PositionAlignment = MapItemAlignment.Center | MapItemAlignment.Bottom,
                };

                // Set texture color
                switch (site.Faction)
                {
                    case Common.AO.Faction.Neutral:
                        //tex.KeyColor = Color.White;
                        tex.KeyColor = Color.LightGray;
                        break;
                    case Common.AO.Faction.Clan:
                        tex.KeyColor = Color.Orange;
                        break;
                    case Common.AO.Faction.Omni:
                        tex.KeyColor = Color.SkyBlue;
                        break;
                    default:
                        tex.KeyColor = Color.Purple;
                        break;
                }

                MapText text = null;
                if (this.ShowNames)
                {
                    text = new MapText
                    {
                        Font = FontType.GuiSmall,
                        Shadow = true,
                        Text = site.Guild
                    };

                    // Set text color
                    switch (site.Faction)
                    {
                        case Common.AO.Faction.Neutral:
                            text.TextColor = Color.LightGray;
                            text.ShadowColor = Color.Black;
                            break;
                        case Common.AO.Faction.Clan:
                            text.TextColor = Color.Orange;
                            text.ShadowColor = Color.Black;
                            break;
                        case Common.AO.Faction.Omni:
                            text.TextColor = Color.SkyBlue;
                            text.ShadowColor = Color.Black;
                            break;
                        default:
                            tex.KeyColor = Color.Purple;
                            break;
                    }
                }



                overlay.MapItems.Add(new MapTextureText(tex, text) { PositionAlignment = MapItemAlignment.Bottom });
            }
        }       
    }
}
