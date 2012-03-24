﻿/*
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
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Demoder.Common.Serialization;

namespace Demoder.PlanetMapViewer
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
            var zoneInfo = this.CoordsFile.Playfields.FirstOrDefault(i => i.ID == zone);
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
            #region sound
            
            // Pixel position - relative world position projected onto map
            // 1: 
            float pixelX = relativeX * (layerInfo.MapRect.Width - layerInfo.MapRect.X) + layerInfo.MapRect.X;

            float pixelY = relativeY * (layerInfo.MapRect.Height - layerInfo.MapRect.Y) - layerInfo.MapRect.Y;

            // Find pixel coord
            return new Vector2(
                (float)Math.Round(pixelX),
                (float)Math.Round(pixelY));
            #endregion
        }

        public Vector2 GetReversePosition(int layer, uint zone, float x, float y)
        {
            #region sound

            var zoneInfo = this.CoordsFile.Playfields.FirstOrDefault(i => i.ID == zone);
            if (zoneInfo == null)
            {
                return Vector2.Zero;
            }

            var layerInfo = this.Layers[layer];


            var pixelPos = new Vector2(x, y);
            #endregion
            float relativeX = pixelPos.X - layerInfo.MapRect.X;
            relativeX /= layerInfo.MapRect.Width - layerInfo.MapRect.X;

            float relativeY = pixelPos.Y + layerInfo.MapRect.Y;
            relativeY /= layerInfo.MapRect.Height - layerInfo.MapRect.Y;
            relativeY = 1 - relativeY;


            float worldX = relativeX * (this.MaxX - this.MinX) / zoneInfo.XScale;
            worldX += this.MinX;
            float worldY = relativeY * (this.MaxY - this.MinY) / zoneInfo.YScale;
            worldY += this.MinY;

            /*return new Vector2(
                worldX - zoneInfo.X,
                worldY - zoneInfo.Y);
             */
            var reversePos = new Vector2(
                (float)Math.Round(worldX - zoneInfo.X),
                (float)Math.Round(worldY - zoneInfo.Y));

            return reversePos;
        }

        private class TmpZoneInfo
        {
            public uint Zone;
            public float X;
            public float Y;
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
            map.Layers = layers.ToArray();
            return map;
        }
        #endregion
    }

    

    public class PlanetMapLayer
    {
        private FileStream binFile;

        public int TextureSize { get; internal set; }
        public Point Size { get; internal set; }
        public Point Tiles { get; internal set; }
        public Rectangle MapRect { get; internal set; }

        private int[,] filePos = null;

        private Texture2D[,] textureMap = null;
        private Texture2D invalidTile;

        private int addedTiles = 0;

        public PlanetMapLayer(string binFile)
        {
            this.binFile = new FileStream(binFile, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public void AddFilePos(int filePos) 
        {
            if (this.filePos == null)
            {
                this.filePos = new int[this.Tiles.Y, this.Tiles.X];
                this.textureMap = new Texture2D[this.Tiles.Y, this.Tiles.X];
            }
            var pos = this.ConvertFilePosNumToSlot(this.addedTiles);
            this.filePos[pos.Y, pos.X] = filePos;
            this.addedTiles++;
        }


        private Point ConvertFilePosNumToSlot(int num)
        {
            if (num == 0) { return Point.Zero; }
            var ret = Point.Zero;
            ret.X = num / this.Tiles.Y;
            ret.Y = num - ret.X * this.Tiles.Y;
            return ret;
        }
        private Texture2D emptyTexture;


        public void Draw(Context context)
        {
            this.Draw(context.SpriteBatch, context.Camera, context.GraphicsDevice, context.UiElements.TileDisplay);
        }

        public void Draw(SpriteBatch batch, Camera camera, GraphicsDevice graphicsDevice, TileDisplay display)
        {
            batch.Begin(
                    SpriteSortMode.Texture,
                    BlendState.AlphaBlend,
                    null, null, null, null,
                    camera.TransformMatrix);
            try
            {
                Vector2 minPos = camera.Position;
                minPos.X /= this.TextureSize;
                minPos.Y /= this.TextureSize;

                Vector2 maxPos = new Vector2(minPos.X, minPos.Y);


                maxPos.X += (display.Width / this.TextureSize) + 2;
                maxPos.Y += (display.Height / this.TextureSize) + 2;

                maxPos.X = Math.Min(maxPos.X, this.Tiles.X);
                maxPos.Y = Math.Min(maxPos.Y, this.Tiles.Y);

                for (int x = (int)Math.Max(minPos.X, 0); x < (int)maxPos.X; x++)
                {
                    for (int y = (int)Math.Max(minPos.Y, 0); y < (int)maxPos.Y; y++)
                    {
                        if (this.textureMap[y, x] == null)
                        {
                            try
                            {
                                int texturePos = this.filePos[y, x];
                                this.binFile.Seek(texturePos, SeekOrigin.Begin);
                                // Read maximum 512kB to find image slice.
                                int size = (int)Math.Min(512 * 1024, this.binFile.Length - texturePos);

                                byte[] b = new byte[size];
                                this.binFile.Read(b, 0, size);

                                var ms = new MemoryStream(b);
                                this.textureMap[y, x] = Texture2D.FromStream(graphicsDevice, ms);
                            }
                            catch (Exception ex)
                            {
                                if (this.invalidTile == null)
                                {
                                    this.invalidTile = new Texture2D(graphicsDevice, this.TextureSize, this.TextureSize, false, SurfaceFormat.Color);
                                    var pixels = new Color[this.TextureSize * this.TextureSize];
                                    for (int i=0; i<pixels.Length; i++){
                                        pixels[i] = Color.White;
                                    }

                                    this.invalidTile.SetData(pixels);
                                }
                                this.textureMap[y, x] = this.invalidTile;
                            }
                        }
      
                        batch.Draw(this.textureMap[y, x],
                            new Rectangle(
                                x * this.TextureSize,
                                y * this.TextureSize,
                                this.TextureSize,
                                this.TextureSize),
                                Color.White);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("PlanetMapLayer->Draw() Exception: {0}", ex.ToString());
            }
            finally
            {
                batch.End();
            }
        }
    }

    public enum MapType  
    {
        Rubika,
        Shadowlands
    }
}