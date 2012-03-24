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
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Demoder.PlanetMapViewer.DataClasses;
using Demoder.PlanetMapViewer.Forms;
using Demoder.PlanetMapViewer.Helpers;
using Demoder.PlanetMapViewer.Properties;

namespace Demoder.PlanetMapViewer.Helpers
{
    public class MapManager
    {
        public Context Context;
        private MapSettings settings { get { return Properties.MapSettings.Default; } }
        #region Members
        public PlanetMap CurrentMap;
        public int CurrentLayerNum = 0;

        /// <summary>
        /// Key: relative path to map file
        /// Value: Pre-loaded planet map info
        /// </summary>
        internal Dictionary<string, PlanetMap> AvailablePlanetMaps = new Dictionary<string, PlanetMap>(StringComparer.InvariantCultureIgnoreCase);
        #endregion

        #region Accessors
        public PlanetMapLayer CurrentLayer
        {
            get
            {
                if (this.CurrentMap == null) { return null; }
                if (this.CurrentMap.Layers.Length - 1 < CurrentLayerNum) { return null; }
                return this.CurrentMap.Layers[CurrentLayerNum];
            }
        }
        private string mapDirectory { get { return Path.Combine(this.settings.AoPath, "cd_image", "textures", "PlanetMap"); } }
        #endregion

        #region Constructors
        public MapManager(Context context)
        {
            this.Context = context;
            
            this.FindAvailableMaps(context.Options.IsMapRubika);


            if (this.Context.Options.IsMapRubika)
            {
                if (!this.SelectRubikaMap())
                {
                    throw new FileNotFoundException("Previously selected planet map (" + this.settings.SelectedRubikaMap + ") does not exist. Attempted to load default RK planet map, but failed to find that as well.", this.settings.SelectedRubikaMap);
                }
            }
            else 
            {
                if (!this.SelectShadowlandsMap())
                {
                    throw new FileNotFoundException("Previously selected planet map (" + this.settings.SelectedShadowlandsMap + ") does not exist. Attempted to load default SL planet map, but failed to find that as well.", this.settings.SelectedShadowlandsMap);
                }
            }
        }
        #endregion

        #region Management

        public bool SelectRubikaMap()
        {
            var ret= this.SelectMap(this.settings.SelectedRubikaMap);
            if (!ret && this.CurrentMap == null)
            {
                ret = this.SelectMap(@"normal\PlanetMapIndexNormal.txt");
            }
            return ret;
        }

        public bool SelectShadowlandsMap()
        {
            var ret= this.SelectMap(this.settings.SelectedShadowlandsMap);
            if (!ret && this.CurrentMap == null)
            {
                ret = this.SelectMap(@"Shadowlands\ShadowlandsMap.txt");
            }
            return ret;
        }


        /// <summary>
        /// Select a map
        /// </summary>
        /// <param name="map"></param>
        /// <returns>True when map selection is changed, otherwise false.</returns>
        public bool SelectMap(string map)
        {
            lock (this.AvailablePlanetMaps)
            {
                if (!this.AvailablePlanetMaps.ContainsKey(map))
                {
                    return false;
                }
                var newMap = this.AvailablePlanetMaps[map];
                if (newMap == this.CurrentMap)
                {
                    return false;
                }


                this.CurrentMap = this.AvailablePlanetMaps[map];
                this.CurrentLayerNum = 0;
                if (this.Context.Camera != null)
                {
                    this.Context.Camera.CenterOnPixel(0, 0);
                    this.Context.Camera.AdjustScrollbarsToLayer();
                }

                for (int i = 0; i < this.Context.UiElements.MapList.Items.Count; i++)
                {
                    if ((this.Context.UiElements.MapList.Items[i] as MapSelectionItem).MapPath.Equals(map, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (i != this.Context.UiElements.MapList.SelectedIndex)
                        {
                            this.Context.UiElements.MapList.SelectedIndex = i;
                        }
                        break;
                    }
                }
                return true;
            }
        }

        public void FindAvailableMaps(bool isRubika)
        {
            lock (this.AvailablePlanetMaps)
            {
                this.AvailablePlanetMaps.Clear();
                this.Context.UiElements.MapList.Items.Clear();
                var candidates = Directory.GetFiles(this.mapDirectory, "*.txt", SearchOption.AllDirectories);
                foreach (var candidate in candidates)
                {
                    try
                    {
                        var map = PlanetMap.FromFile(this.mapDirectory, candidate);
                        if (map == null) { continue; }
                        if (String.IsNullOrEmpty(map.Name)) { continue; }
                        if (map.Layers.Length == 0) { continue; }
                        if (map.Type == MapType.Rubika && !isRubika) { continue; }
                        if (map.Type == MapType.Shadowlands && isRubika) { continue; }
                        var pathName = candidate.Remove(0, this.mapDirectory.Length + 1);
                        this.AvailablePlanetMaps.Add(pathName, map);
                        this.Context.UiElements.MapList.Items.Add(new MapSelectionItem { MapName = map.Name.Trim('"', ' '), MapPath = pathName });
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        public void Load(string map)
        {
            this.CurrentMap = PlanetMap.FromFile(this.mapDirectory, map);
        }

        public bool ZoomIn()
        {
            if (this.CurrentLayerNum < this.CurrentMap.Layers.Length - 1)
            {
                var relPos = this.Context.Camera.RelativePosition();
                this.CurrentLayerNum++;
                this.Context.Camera.AdjustScrollbarsToLayer();
                this.Context.Camera.CenterOnRelativePosition(relPos);
                return true;
            }
            return false;
        }
        public bool ZoomOut()
        {
            if (this.CurrentLayerNum > 0)
            {
                var relPos = this.Context.Camera.RelativePosition();
                this.CurrentLayerNum--;
                this.Context.Camera.AdjustScrollbarsToLayer();
                this.Context.Camera.CenterOnRelativePosition(relPos);
                return true;
            }
            return false;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="x">Position in zone</param>
        /// <param name="y">Position in zone</param>
        /// <returns></returns>
        public Microsoft.Xna.Framework.Vector2 GetPosition(uint zone, float x, float y)
        {
            return this.CurrentMap.GetPosition(this.CurrentLayerNum, zone, x, y);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="x">Pixel</param>
        /// <param name="y">Pixel</param>
        /// <returns></returns>
        public Microsoft.Xna.Framework.Vector2 GetReversePosition(uint zone, float x, float y)
        {
            return this.CurrentMap.GetReversePosition(this.CurrentLayerNum, zone, x, y);
        }
    }
}
