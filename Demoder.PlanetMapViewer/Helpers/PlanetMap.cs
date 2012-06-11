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
using Demoder.Common.Serialization;
using Demoder.PlanetMapViewer.DataClasses;
using Demoder.PlanetMapViewer.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Demoder.PlanetMapViewer.Helpers
{
    public class PlanetMap
    {
        public string Name { get; private set; }
        public MapType Type { get; private set; }
        public MapCoords CoordsFile;
        public PlanetMapLayer[] Layers;

        #region Accessors
        public int MinX { get { return 31022; } }
        public int MaxX { get { return 49770; } }

        public int MinY { get { return 24880; } }
        public int MaxY { get { return 48996; } }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        private PlanetMap()
        {
                        
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="layer">Layer number</param>
        /// <param name="zone">Zone</param>
        /// <param name="x">Zone position X</param>
        /// <param name="y">Zone position Y</param>
        /// <returns></returns>
        public Vector2 GetPosition(int layer, uint zone, float x, float y)
        {
            // Get zone info.
            var zoneInfo = this.CoordsFile[zone];
            //var zoneInfo = this.CoordsFile.Playfields.FirstOrDefault(i => i.ID == zone);
            if (zoneInfo == null)
            {
                return Vector2.Zero;
            }

            var layerInfo = this.Layers[layer];


            /*
             * On XScale/ZScale:
             * Logically, a zones x/z scale should only affect local(within-zone) coordinates... but no.
             * Actual behaviour: X/Zscale affects the zone+character total world position relative to Min/Max world positions.
             * Thus: Apply x/z scale to relativePosition.
             *  - Demoder
              */

            // Character world position (TOTAL in excep spreadsheet)
            var worldPos = new Vector2(
                zoneInfo.X + x,
                zoneInfo.Y + y);

            // Relative world position (float between 0 and 1)  (LOCAL in spreadsheet)
            // 1: Remove anything before "beginning of world" coordinate from the equation.
            // 2: Divide result by the size of the world.
            float relativeX = zoneInfo.XScale * (worldPos.X - this.MinX) / (this.MaxX - this.MinX);

            float relativeY = 1 - (zoneInfo.YScale * (worldPos.Y - this.MinY) / (this.MaxY - this.MinY));
            
            // Pixel position - relative world position projected onto map
            // 1: 
            float pixelX = relativeX * (layerInfo.MapRect.Width - layerInfo.MapRect.X) + layerInfo.MapRect.X;

            float pixelY = relativeY * (layerInfo.MapRect.Height - layerInfo.MapRect.Y) - layerInfo.MapRect.Y;

            // Find pixel coord
            return new Vector2(
                (float)Math.Round(pixelX * API.State.Magnification),
                (float)Math.Round(pixelY * API.State.Magnification));
        }

        public Vector2 GetReversePosition(int layer, uint zone, float x, float y)
        {
            //var zoneInfo = this.CoordsFile.Playfields.FirstOrDefault(i => i.ID == zone);
            var zoneInfo = this.CoordsFile[zone];
            if (zoneInfo == null)
            {
                return Vector2.Zero;
            }

            var layerInfo = this.Layers[layer];


            var pixelPos = new Vector2(
                (float)Math.Round(x / API.State.Magnification), 
                (float)Math.Round(y / API.State.Magnification));

            float relativeX = pixelPos.X - layerInfo.MapRect.X;
            relativeX /= layerInfo.MapRect.Width - layerInfo.MapRect.X;

            float relativeY = pixelPos.Y + layerInfo.MapRect.Y;
            relativeY /= layerInfo.MapRect.Height - layerInfo.MapRect.Y;
            relativeY = 1 - relativeY;


            float worldX = relativeX * (this.MaxX - this.MinX) / zoneInfo.XScale;
            worldX += this.MinX;
            float worldY = relativeY * (this.MaxY - this.MinY) / zoneInfo.YScale;
            worldY += this.MinY;

            var reversePos = new Vector2(
                (float)Math.Round(worldX - zoneInfo.X),
                (float)Math.Round(worldY - zoneInfo.Y));

            return reversePos;
        }

        public void UnloadAllTextures()
        {
            foreach (var layer in this.Layers)
            {
                layer.UnloadAllTextures();
            }
        }

        #region Static methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapDir">Full path to map directory (cd_image/textures/Planetmap/)</param>
        /// <param name="mapFile">Full path to map file (AoRK/AoRK.txt)</param>
        /// <returns></returns>
        public static PlanetMap FromFile(string mapDir, string mapFile)
        {
            var map = new PlanetMap();
            var layers = new List<PlanetMapLayer>();

            PlanetMapLayer currentLayer = null;
            using (var reader = new StreamReader(mapFile))
            {
                bool readingLayer = false;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine().Trim();
                    if (String.IsNullOrEmpty(line)) { continue; }
                    var words = line.Split(' ');
                    if (!readingLayer)
                    {
                        // Name
                        if (words[0] == "Name")
                        {
                            map.Name = line.Substring("Name".Length + 1);
                            continue;
                        }
                        if (words[0] == "Type")
                        {
                            MapType mapType;
                            Enum.TryParse<MapType>(words[1], out mapType);
                            map.Type = mapType;
                            continue;
                        }
                        if (words[0] == "CoordsFile")
                        {
                            map.CoordsFile = Xml.Deserialize<MapCoords>(new FileInfo(Path.Combine(mapDir, words[1])), false);
                            continue;
                        }
                    }

                    if (words[0] == "File")
                    {
                        // Starting a new layer.
                        currentLayer = new PlanetMapLayer(Path.Combine(mapDir, words[1]));
                        layers.Add(currentLayer);
                        readingLayer = true;
                        continue;
                    }
                    if (currentLayer != null)
                    {
                        if (words[0] == "TextureSize")
                        {
                            currentLayer.TextureSize = int.Parse(words[1]);
                            continue;
                        }
                        if (words[0] == "Size")
                        {
                            currentLayer.Size = new Point(
                                int.Parse(words[1]),
                                int.Parse(words[2]));
                            continue;
                        }
                        if (words[0]=="Tiles") {
                            currentLayer.Tiles = new Point(
                                int.Parse(words[1]),
                                int.Parse(words[2]));
                            continue;
                        }
                        if (words[0] == "MapRect")
                        {
                            currentLayer.MapRect = new Rectangle(
                                int.Parse(words[1]),
                                int.Parse(words[2]),
                                int.Parse(words[3]),
                                int.Parse(words[4]));
                            continue;
                        }
                        if (words[0] == "FilePos")
                        {
                            currentLayer.AddFilePos(int.Parse(words[1]));
                        }
                    }
                }
            }

            if (map.CoordsFile == null)
            {
                try
                {
                    map.CoordsFile = Xml.Deserialize<MapCoords>(new FileInfo(Path.Combine(mapDir, "MapCoordinates.xml")), false);
                }
                catch { }
            }

            // Is it still invalid?
            if (map.CoordsFile == null)
            {
                var ms = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(Properties.Resources.MapCoordinates));
                map.CoordsFile = Xml.Deserialize<MapCoords>(ms,true);
            }

            map.Layers = layers.ToArray();
            return map;
        }
        #endregion
    }


    public class MapTextureInfo
    {
        public Texture2D Texture { get; set; }
        public uint Used { get; set; }
    }

    public class PlanetMapLayer
    {
        private FileStream binFile;

        public int TextureSize { get; internal set; }
        public Point Size { get; internal set; }
        public Point Tiles { get; internal set; }
        public Rectangle MapRect { get; internal set; }

        private int[,] filePos = null;

        private MapTextureInfo[,] textureMap = null;
        
        private Texture2D invalidTile;

        private int addedTiles = 0;

        public Stopwatch LastDraw {get; private set;}

        public PlanetMapLayer(string binFile)
        {
            this.binFile = new FileStream(binFile, FileMode.Open, FileAccess.Read, FileShare.Read);
            this.LastDraw = new Stopwatch();
        }

        public void AddFilePos(int filePos) 
        {
            if (this.filePos == null)
            {
                this.filePos = new int[this.Tiles.Y, this.Tiles.X];
                this.InitializeTextureMap();
            }
            var pos = this.ConvertFilePosNumToSlot(this.addedTiles);
            this.filePos[pos.Y, pos.X] = filePos;
            this.addedTiles++;
        }

        private void InitializeTextureMap()
        {
            if (this.textureMap != null) { return; }
            this.textureMap = new MapTextureInfo[this.Tiles.Y, this.Tiles.X];
            for (int x = 0; x < this.Tiles.X; x++)
            {
                for (int y = 0; y < this.Tiles.Y; y++)
                {
                    this.textureMap[y, x] = new MapTextureInfo();
                }
            }
        }

        internal void UnloadAllTextures()
        {
            for (int x = 0; x < this.Tiles.X; x++)
            {
                for (int y = 0; y < this.Tiles.Y; y++)
                {
                    if (this.textureMap[y, x].Texture == null) { continue; }
                    this.textureMap[y, x].Texture.Dispose();
                    this.textureMap[y, x].Texture = null;

                }
            }
        }

        private Point ConvertFilePosNumToSlot(int num)
        {
            if (num == 0) { return Point.Zero; }
            var ret = Point.Zero;
            ret.X = num / this.Tiles.Y;
            ret.Y = num - ret.X * this.Tiles.Y;
            return ret;
        }

        public void Draw()
        {
            this.LastDraw.Restart();
            var batch = API.SpriteBatch;
            var camera = API.Camera;
            var graphicsDevice = API.GraphicsDevice;
            var display = API.UiElements.TileDisplay;

            var txz = (float)Math.Round(this.TextureSize * API.State.Magnification);

            batch.Begin(
                    SpriteSortMode.Texture,
                    BlendState.AlphaBlend,
                    new SamplerState { Filter = TextureFilter.MinLinearMagPointMipLinear },
                    null, null, null,
                    camera.TransformMatrix);



            try
            {
                Vector2 minPos = camera.Position;
                minPos.X /= txz;
                minPos.Y /= txz;
                minPos.X = MathHelper.Clamp(minPos.X - 2, 0, this.Tiles.X);
                minPos.Y = MathHelper.Clamp(minPos.Y - 2, 0, this.Tiles.Y);

                Vector2 maxPos = new Vector2(minPos.X, minPos.Y);
                maxPos.X += (display.Width / txz) + 2;
                maxPos.Y += (display.Height / txz) + 2;
                maxPos.X = MathHelper.Clamp(maxPos.X + 2, 0, this.Tiles.X);
                maxPos.Y = MathHelper.Clamp(maxPos.Y + 2, 0, this.Tiles.Y);



                var maxFrameAge = (int)(Properties.GeneralSettings.Default.FramesPerSecond * 120);

                for (int x = 0; x < this.Tiles.X; x++)
                {
                    for (int y = 0; y < this.Tiles.Y; y++)
                    {
                        // Tag all unused tiles
                        if (x < minPos.X || x > maxPos.X || y < minPos.Y || y > maxPos.Y)
                        {
                            if (this.textureMap[y, x].Texture == null) { continue; }
                            this.textureMap[y, x].Used++;

                            // Unload tiles which haven't been used for 120 seconds.
                            if (this.textureMap[y, x].Used > maxFrameAge)
                            {
                                this.textureMap[y, x].Texture.Dispose();
                                this.textureMap[y, x].Texture = null;
                            }
                            continue;
                        }

                        // We're supposed to use this tile
                        this.textureMap[y, x].Used = 0;
                        this.LoadTile(graphicsDevice, x, y);

                        batch.Draw(this.textureMap[y, x].Texture,
                            new Rectangle(
                                (int)(x * txz),
                                (int)(y * txz),
                                (int)(txz),
                                (int)(txz)),
                                Color.White);
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                Program.WriteLog("PlanetMapLayer->Draw() Exception: {0}", ex.ToString());
            }

            catch (Exception ex)
            {
                Program.WriteLog("PlanetMapLayer->Draw() Exception: {0}", ex.ToString());
            }
            finally
            {
                batch.End();
            }
        }

        /// <summary>
        /// Loads a single tile
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void LoadTile(GraphicsDevice graphicsDevice, int x, int y)
        {
            this.InitializeTextureMap();
            if (this.textureMap[y, x].Texture != null) { return; }

            try
            {
                int texturePos = this.filePos[y, x];
                this.binFile.Seek(texturePos, SeekOrigin.Begin);
                
                // Read maximum 512kB to find image slice.
                int size = (int)Math.Min(512 * 1024, this.binFile.Length - texturePos);

                var b = new byte[size];
                this.binFile.Read(b, 0, size);
                var ms = new MemoryStream(b, false);
                var tex = Texture2D.FromStream(graphicsDevice, ms);
                this.textureMap[y, x].Texture = tex;
            }
            catch (Exception ex)
            {
                if (this.invalidTile == null)
                {
                    this.invalidTile = new Texture2D(graphicsDevice, this.TextureSize, this.TextureSize, false, SurfaceFormat.Color);
                    var pixels = new Color[this.TextureSize * this.TextureSize];
                    for (int i = 0; i < pixels.Length; i++)
                    {
                        pixels[i] = Color.White;
                    }

                    this.invalidTile.SetData(pixels);
                }
                this.textureMap[y, x] = new MapTextureInfo { Texture = this.invalidTile };
            }
        }
    }
}
