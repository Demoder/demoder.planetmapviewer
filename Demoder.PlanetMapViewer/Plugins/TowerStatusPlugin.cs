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
using System.ComponentModel;
using System.Linq;
using System.Text;
using Demoder.Common.AO;
using Demoder.Common.AO.Towerwars.DataClasses;
using Demoder.Common.Attributes;
using Demoder.Common.Cache;
using Demoder.Common.Extensions;
using Demoder.Common.Serialization;
using Demoder.Common.Web;
using Demoder.PlanetMapViewer.DataClasses;
using Demoder.PlanetMapViewer.Helpers;
using Demoder.PlanetMapViewer.PmvApi;
using Microsoft.Xna.Framework;

namespace Demoder.PlanetMapViewer.Plugins
{
    [Plugin("TowerStatus", 60)]
    [Description("Displays current tower war status on the planet map")]
    public class TowerStatusPlugin : IPlugin
    {
        #region Settings
        [Setting(true)]
        public bool ShowNames { get; set; }

        [Setting(true)]
        public bool ShowRecentAttacks { get; set; }

        [Setting(LoadedFont.Silkscreen7)]
        public LoadedFont RecentAttacksFont { get; set; }

        [Setting(LoadedFont.Silkscreen7)]
        public LoadedFont LcaOwnerFont { get; set; }

        [Setting(1)]
        public int LcaMinLevel { get; set; }

        [Setting(300)]
        public int LcaMaxLevel { get; set; }
        #endregion

        private const string twTowerDistribution="http://towerwars.info/m/TowerDistribution.php";
        private const string twHistorySearch = "http://towerwars.info/m/HistorySearch.php";

        public TowerStatusPlugin()
        {
            API.MapManager.MapChangedEvent += new Action(this.HandleMapChangedEvent);
            API.XmlCache.Create<TowerSites>(new TimeSpan(0,5,0));
            API.XmlCache.Create<TowerAttacks>(new TimeSpan(0,1,0));

            API.AoHook.TrackedDimensionChanged += AoHook_TrackedDimensionChanged;
        }

        void AoHook_TrackedDimensionChanged()
        {
            API.PluginManager.RedrawLayers(this.GetType());
        }

        public void Dispose()
        {
            API.MapManager.MapChangedEvent -= new Action(this.HandleMapChangedEvent);
        }

        private void HandleMapChangedEvent()
        {
            API.PluginManager.ChangeLayerStatus<TowerStatusPlugin>("LcaInfo", API.MapManager.CurrentMap.Type == MapType.Rubika);
        }   

        public IEnumerable<MapOverlay> GetCustomOverlay()
        {
            yield return this.GenerateTowerFieldInfo(Dimension.RubiKa);
            yield return this.GenerateRecentAttackInfo(Dimension.RubiKa);
        }

        private MapOverlay GenerateRecentAttackInfo(Dimension dimension)
        {
            if (!this.ShowRecentAttacks) { return null; }
            var overlay = new GuiTableOverlay
            {
                HeaderVisible = false,
                Position = new Point(5,5),
                ColumnSpacing = 15,
                RowSpacing = 0,
            };

            // Get tower data.
            var qb = new UriQueryBuilder();
            {
                dynamic q = qb;
                q.d = (int)dimension;
                q.output = "xml";
                q.limit = 5;
                q.sortorder = "desc";
                q.chopmethod = "last";
                q.starttime = Demoder.Common.Misc.Unixtime(DateTime.Now.Subtract(new TimeSpan(6, 0, 0)));
            }

            var towerData = API.XmlCache.Get<TowerAttacks>().Request(
                Common.CacheFlags.Default,
                qb.ToUri(new Uri(twHistorySearch)),
                dimension.ToString());

            if (towerData == null)
            {
                overlay.MapItems.Add(new MapText
                {
                    Position = new PositionDefinition(5, 5),
                    PositionAlignment = MapItemAlignment.Top | MapItemAlignment.Left,
                    TextColor = Color.White,
                    OutlineColor = Color.Black,
                    Outline = true,
                    Font = API.Content.Fonts.GetFont(this.RecentAttacksFont),
                    Text = "Error fetching recent attacks for " + dimension.ToString()
                });
                if (dimension == Common.AO.Dimension.Unknown)
                {
                    (overlay.MapItems.Last() as MapText).Text = "Please track a character, or select dimension in plugin configuration.";
                }
                return overlay;
            }
            if (towerData.Attacks.Count == 0)
            {
                overlay.MapItems.Add(new MapText
                {
                    Position = new PositionDefinition(5, 5),
                    PositionAlignment = MapItemAlignment.Top | MapItemAlignment.Left,
                    TextColor = Color.White,
                    OutlineColor = Color.Black,
                    Outline = true,
                    Font = API.Content.Fonts.GetFont(this.RecentAttacksFont),
                    Text = "No recent attacks on " + dimension.ToString()
                });
                return overlay;
            }

            // Define headers.
            var headerTexts = new string[]{
                "Time",
                "Zone",
                "#",
                "Nick",
                "Org"
            };
            for (int i = 0; i < 5; i++)
            {
                overlay.Headers[i] = new GuiTableHeader
                {
                    AutoSize = true,
                    Alignment = MapItemAlignment.Left | MapItemAlignment.Top,
                    Content = new MapText
                    {
                        Font = API.Content.Fonts.GetFont(this.RecentAttacksFont),
                        Text = headerTexts[i],
                        TextColor = Color.White,
                        Outline = true,
                        OutlineColor = Color.Black,
                        PositionAlignment = MapItemAlignment.Center | MapItemAlignment.Top
                    }
                };
            }

            overlay.Title = new MapText
            {
                Position = new PositionDefinition(5, 5),
                PositionAlignment = MapItemAlignment.Top | MapItemAlignment.Left,
                TextColor = Color.White,
                OutlineColor = Color.Black,
                Outline = true,
                Font = API.Content.Fonts.GetFont(this.RecentAttacksFont),
                Text = "Recent tower attacks on " + dimension.ToString()
            };

            int row = 0;
            var font = API.Content.Fonts.GetFont(this.RecentAttacksFont);
            foreach (var attack in towerData.Attacks)
            {

                // time
                overlay[row, 0] = new MapText
                {
                    Text = attack.Time.ToShortTimeString(),
                    Font = font,
                    TextColor = Color.White,
                    OutlineColor = Color.Black,
                    Outline = true,
                };

                // Attacked site ZONE
                overlay[row, 1] = new MapText
                {
                    Text = attack.ZoneShortName,
                    Font = font,
                    TextColor = this.GetFactionColor(attack.DefenderFaction),
                    OutlineColor = Color.Black,
                    Outline = true,
                };
                                
                // Attacked site ID
                overlay[row, 2] = new MapText
                {
                    Text = String.Format("x{0}", attack.SiteID),
                    Font = font,
                    TextColor = this.GetFactionColor(attack.DefenderFaction),
                    OutlineColor = Color.Black,
                    Outline = true,
                };


                // Attacker nickname
                overlay[row, 3] = new MapText
                {
                    Text = attack.AttackerNickName,
                    Font = font,
                    TextColor = this.GetFactionColor(attack.AttackerFaction),
                    OutlineColor = Color.Black,
                    Outline = true,
                };

                // Attacker org
               overlay[row, 4] = new MapText
                {
                    Font = font,
                    TextColor = this.GetFactionColor(attack.AttackerFaction),
                    OutlineColor = Color.Black,
                    Outline = true,
                };
                
                if (!String.IsNullOrWhiteSpace(attack.AttackerGuildName))
                {
                    (overlay[row, 4] as MapText).Text = attack.AttackerGuildName;
                }
                else 
                {
                    (overlay[row, 4] as MapText).Text = "-";
                }

                row++;
            }
            return overlay.ToGuiOverlay();
        }
        
        private MapOverlay GenerateTowerFieldInfo(Dimension dimension)
        {
            var overlay = new MapOverlay
            {
                Name = "LcaInfo",
            };

            // Get tower data.
            var qb = new UriQueryBuilder();
            {
                dynamic q = qb;
                q.d = (int)dimension;
                q.output = "xml";
                q.limit = 300;
                q.minlevel = this.LcaMinLevel;
                q.maxlevel = this.LcaMaxLevel;
            }

            var towerData = API.XmlCache.Get<TowerSites>().Request(
                Demoder.Common.CacheFlags.Default,
                qb.ToUri(new Uri(twTowerDistribution)),
                dimension.ToString(), 
                this.LcaMinLevel.ToString(), 
                this.LcaMaxLevel.ToString());

            if (towerData == null) { return null; }
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
                        Font = API.Content.Fonts.GetFont(this.LcaOwnerFont),
                        Outline = true,
                        Text = site.Guild
                    };

                    // Set text color
                    switch (site.Faction)
                    {
                        case Common.AO.Faction.Neutral:
                            text.TextColor = Color.LightGray;
                            text.OutlineColor = Color.Black;
                            break;
                        case Common.AO.Faction.Clan:
                            text.TextColor = Color.Orange;
                            text.OutlineColor = Color.Black;
                            break;
                        case Common.AO.Faction.Omni:
                            text.TextColor = Color.SkyBlue;
                            text.OutlineColor = Color.Black;
                            break;
                        default:
                            tex.KeyColor = Color.Purple;
                            break;
                    }
                }



                overlay.MapItems.Add(new MapTextureText(tex, text) { PositionAlignment = MapItemAlignment.Bottom });
            }
            return overlay;
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
    }
}
