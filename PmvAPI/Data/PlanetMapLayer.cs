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
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Demoder.PmvAPI.Data
{
    public class PlanetMapLayer
    {
        private FileStream binFile;

        public int TextureSize { get; internal set; }
        public Point Size { get; internal set; }
        public Point Tiles { get; internal set; }
        public Rectangle MapRect { get; internal set; }

        private int[,] filePos = null;

        private Texture2D[,] textureMap = null;
        private uint[,] textureUsedMap = null;

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
                this.textureUsedMap = new uint[this.Tiles.Y, this.Tiles.X];
            }
            var pos = this.ConvertFilePosNumToSlot(this.addedTiles);
            this.filePos[pos.Y, pos.X] = filePos;
            this.addedTiles++;
        }

        internal void UnloadAllTextures()
        {
            for (int x = 0; x < this.Tiles.X; x++)
            {
                for (int y = 0; y < this.Tiles.Y; y++)
                {
                    if (this.textureMap[y, x] == null) { continue; }
                    this.textureMap[y, x].Dispose();
                    this.textureMap[y, x] = null;

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

        /* 
        public void Draw(Context context)
        {
            this.LastDraw.Restart();
            var batch = context.SpriteBatch;
            var camera = context.Camera;
            var graphicsDevice = context.GraphicsDevice;
            var display = context.UiElements.TileDisplay;

            var txz = (float)Math.Round(this.TextureSize * context.State.Magnification);

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



                for (int x = 0; x < this.Tiles.X; x++)
                {
                    for (int y = 0; y < this.Tiles.Y; y++)
                    {

                        // Tag all unused tiles
                        if (x < minPos.X || x > maxPos.X || y < minPos.Y || y > maxPos.Y)
                        {
                            this.textureUsedMap[y, x]++;

                            // Unload tiles which haven't been used for 120 seconds.
                            if (this.textureMap[y, x] != null && this.textureUsedMap[y, x] > (Properties.GeneralSettings.Default.FramesPerSecond * 120))
                            {
                                this.textureMap[y, x].Dispose();
                                this.textureMap[y, x] = null;
                            }
                            continue;
                        }


                        // We're supposed to use this tile
                        this.textureUsedMap[y, x] = 0;
                        this.LoadTile(graphicsDevice, x, y);

                        batch.Draw(this.textureMap[y, x],
                            new Rectangle(
                                (int)(x * txz),
                                (int)(y * txz),
                                (int)(txz),
                                (int)(txz)),
                                Color.White);
                    }
                }
            }
            catch (Exception ex)
            {
                context.ErrorLog.Enqueue(ex.ToString());
                Console.WriteLine("PlanetMapLayer->Draw() Exception: {0}", ex.ToString());
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
            if (this.textureMap == null) { this.textureMap = new Texture2D[this.Tiles.Y, this.Tiles.X]; }
            if (this.textureMap[y, x] != null) { return; }

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
                this.textureMap[y, x] = tex;
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
                this.textureMap[y, x] = this.invalidTile;
            }
        }
        */
    }
}
