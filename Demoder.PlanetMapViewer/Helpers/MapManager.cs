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
using System.Threading.Tasks;

namespace Demoder.PlanetMapViewer.Helpers
{
    public class MapManager
    {
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

        #region Events
        public event Action ZoomInEvent;
        public event Action ZoomOutEvent;
        #endregion

        #region Constructors
        public MapManager()
        {
        }

        public void Initialize()
        {
            // Locate maps
            this.FindAvailableMaps(Context.State.MapType);

            if (this.SelectMap(Context.State.MapType)) { return; }
            // Couldn't find selected map. try finding substitute.
            if (this.AvailablePlanetMaps.Count == 0)
            {
                this.ShowMapNotFoundException(Context.State.MapType.ToString());
                return;
            }
            this.SelectMap(this.AvailablePlanetMaps.First().Key);
            return;
        }

        /// <summary>
        /// Display a preformatted message box for map errors.
        /// </summary>
        /// <param name="mapName"></param>
        /// <param name="mapType"></param>
        private void ShowMapNotFoundException(string mapType)
        {
            MessageBox.Show(
                String.Format("There are no valid {0} maps in the Anarchy Online planetmap directory.", mapType),
                "Demoder's Planet Map Viewer: No maps found",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        #endregion

        #region Management

        public bool SelectMap(MapType mapType)
        {
            string map;
            string defaultMap;

            switch (mapType)
            {
                case MapType.Rubika:
                    map = this.settings.SelectedRubikaMap;
                    defaultMap = @"normal\PlanetMapIndexNormal.txt";
                    break;
                default:
                case MapType.Shadowlands:
                    map = this.settings.SelectedShadowlandsMap;
                    defaultMap = @"Shadowlands\ShadowlandsMap.txt";
                    break;
            }

            if (this.SelectMap(map))
            {
                return true;
            }
            return this.SelectMap(defaultMap);
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
                var oldMap = this.CurrentMap;

                this.SaveCurrentMapZoomLevel();

                this.CurrentMap = newMap;
                // Unload map textures.
                if (oldMap != null)
                {
                    oldMap.UnloadAllTextures();
                }
                SetCurrentMapZoomLevel(newMap);
                
                if (Context.Camera != null)
                {
                    Context.Camera.CenterOnPixel(this.CurrentLayer.Size.X / 2, this.CurrentLayer.Size.Y / 2);
                    Context.Camera.AdjustScrollbarsToLayer();
                }

                for (int i = 0; i < Context.UiElements.MapList.Items.Count; i++)
                {
                    if ((Context.UiElements.MapList.Items[i] as MapSelectionItem).MapPath.Equals(map, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (i != Context.UiElements.MapList.SelectedIndex)
                        {
                            Context.UiElements.MapList.SelectedIndex = i;
                        }
                        break;
                    }
                }
                return true;
            }
        }

        private void SetCurrentMapZoomLevel(PlanetMap newMap)
        {
            if (newMap.Type == MapType.Rubika)
            {
                this.CurrentLayerNum = Math.Min(Properties.MapSettings.Default.SelectedRubikaMapLayer, newMap.Layers.Length - 1);
            }
            else
            {
                this.CurrentLayerNum = Math.Min(Properties.MapSettings.Default.SelectedShadowlandsMapLayer, newMap.Layers.Length - 1);
            }
        }

        private void SaveCurrentMapZoomLevel()
        {
            // Save current map layer setting, if suitable.
            if (this.CurrentMap == null) { return; }

            if (this.CurrentMap.Type == MapType.Rubika)
            {
                Properties.MapSettings.Default.SelectedRubikaMapLayer = this.CurrentLayerNum;
                Properties.MapSettings.Default.Save();
            }

            if (this.CurrentMap.Type == MapType.Shadowlands)
            {
                Properties.MapSettings.Default.SelectedShadowlandsMapLayer = this.CurrentLayerNum;
                Properties.MapSettings.Default.Save();
            }
        }

        public void FindAvailableMaps(MapType mapType)
        {
            lock (this.AvailablePlanetMaps)
            {
                this.AvailablePlanetMaps.Clear();
                Context.UiElements.MapList.Items.Clear();
                var candidates = Directory.GetFiles(this.mapDirectory, "*.txt", SearchOption.AllDirectories);
                foreach (var candidate in candidates)
                {
                    try
                    {
                        var map = PlanetMap.FromFile(this.mapDirectory, candidate);
                        if (map == null) { continue; }
                        if (String.IsNullOrEmpty(map.Name)) { continue; }
                        if (map.Layers.Length == 0) { continue; }
                        if (map.Type != mapType) { continue; }
                        var pathName = candidate.Remove(0, this.mapDirectory.Length + 1);
                        this.AvailablePlanetMaps.Add(pathName, map);
                        Context.UiElements.MapList.Items.Add(new MapSelectionItem { MapName = map.Name.Trim('"', ' '), MapPath = pathName, Type=map.Type });
                    }
                    catch(Exception ex)
                    {
                        Context.ErrorLog.Enqueue(ex.ToString());
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
                var relPos = Context.Camera.RelativePosition();
                this.CurrentLayerNum++;
                Context.Camera.AdjustScrollbarsToLayer();
                Context.Camera.CenterOnRelativePosition(relPos);
                this.CurrentMap.Layers[this.CurrentLayerNum - 1].UnloadAllTextures();
                if (this.ZoomInEvent != null)
                {
                    Task.Factory.StartNew(this.ZoomInEvent);
                }
                return true;
            }
            return false;
        }
        public bool ZoomOut()
        {
            if (this.CurrentLayerNum > 0)
            {
                var relPos = Context.Camera.RelativePosition();
                this.CurrentLayerNum--;
                Context.Camera.AdjustScrollbarsToLayer();
                Context.Camera.CenterOnRelativePosition(relPos);
                this.CurrentMap.Layers[this.CurrentLayerNum + 1].UnloadAllTextures();
                if (this.ZoomOutEvent != null)
                {
                    Task.Factory.StartNew(this.ZoomOutEvent);
                }
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
