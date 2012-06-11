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
using Demoder.PlanetMapViewer.DataClasses;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Demoder.Common.Serialization;

namespace Demoder.PlanetMapViewer.Helpers
{
    public class FrameDrawer
    {
        public bool IsBatchBegun { get; private set; }

        public FrameDrawer()
        {
            this.IsBatchBegun = false;
        }


        public void Draw(IEnumerable<IMapItem> mapItems)
        {
            var worldItems = mapItems.Where(i => i.Position.Type == DrawMode.World).ToArray();
            var hudItems = mapItems.Where(i => i.Position.Type == DrawMode.ViewPort).ToArray();

            if (worldItems.Length > 0)
            {
                this.Draw(worldItems, DrawMode.World);
            }
            if (hudItems.Length > 0)
            {
                this.Draw(hudItems, DrawMode.ViewPort);
            }
        }

        private void Draw(IEnumerable<IMapItem> mapItems, DrawMode drawMode)
        {
            var bufferedItems = new List<IMapItem>();
            MapItemType lastType = default(MapItemType);

            foreach (var item in mapItems)
            {
                if (item == null) { continue; }
                if (item.Type != lastType && bufferedItems.Count != 0)
                {
                    this.Draw(bufferedItems, drawMode, lastType);
                    bufferedItems.Clear();
                }
                bufferedItems.Add(item);
                lastType = item.Type;
            }
            this.Draw(bufferedItems, drawMode, lastType);
        }

        public void Draw(IEnumerable<IMapItem> items, DrawMode drawMode, MapItemType itemType)
        {
            if (items.Count() == 0) { return; }
            switch (itemType)
            {
                case MapItemType.Texture:
                    this.DrawTexture(items, drawMode);
                    break;
                case MapItemType.SpriteFont:
                    this.DrawText(items, drawMode);
                    break;
                case MapItemType.TextureWithAttachedSpriteFont:
                    this.DrawTextureWithSpriteFont(items, drawMode);
                    break;
            }
        }

        private void DrawTextureWithSpriteFont(IEnumerable<IMapItem> items, DrawMode drawMode)
        {
            var realItems = new List<IMapItem>();

            foreach (var item in items)
            {
                if (item is MapTextureText)
                {
                   realItems.AddRange((item as MapTextureText).ToMapItems());
                }
            }
            this.Draw(realItems, drawMode);
        }


        /// <summary>
        /// Draws text onto the tile display. NOTICE: Starts its own SpriteBatch!
        /// </summary>
        /// <param name="texts"></param>
        /// <param name="worldText">If set to true, positions are relative to map area. If set to false, positions are relative to viewport area.</param>
        public void DrawText(IEnumerable<IMapItem> items, DrawMode drawMode = DrawMode.World)
        {
            try
            {
                #region Shadow
                this.SpriteBatchBegin(drawMode);
                try
                {
                    foreach (var item in items)
                    {
                        if (item.Type != MapItemType.SpriteFont) { continue; }
                        var sd = item as MapText;
                        if (String.IsNullOrEmpty(sd.Text)) { continue; }
                        if (sd.Font == null) { continue; }
                        var textSize = sd.Size;
                        var pos = GetRealPosition(item);

                        if (sd.Position.Type == DrawMode.World && !this.IsInsideViewport(pos, sd.Size))
                        {
                            continue;
                        }

                        for (int x = -1; x <= 1; x++)
                        {
                            for (int y = -1; y <= 1; y++)
                            {
                                if (x == 0 && y == 0) { continue; }

                                API.SpriteBatch.DrawString(
                                    sd.Font,
                                    sd.Text,
                                    new Vector2(pos.X + x, pos.Y + y),
                                    sd.OutlineColor
                                    );
                            }
                        }
                            pos.X++;
                        pos.Y++;
                        
                    }
                }
                catch (Exception ex)
                {
                    Program.WriteLog(ex);
                    throw ex;
                }
                finally
                {
                    this.SpriteBatchEnd();
                }
                #endregion

                
                #region Normal
                this.SpriteBatchBegin(drawMode);
                try
                {
                    foreach (var item in items)
                    {
                        if (item.Type != MapItemType.SpriteFont) { continue; }
                        var sd = item as MapText;
                        if (String.IsNullOrEmpty(sd.Text)) { continue; }
                        if (sd.Font == null) { continue; }
                        var textSize = sd.Size;
                        var pos = GetRealPosition(item);

                        if (sd.Position.Type == DrawMode.World && !this.IsInsideViewport(pos, sd.Size))
                        {
                            continue;
                        }

                        API.SpriteBatch.DrawString(
                            sd.Font,
                            sd.Text,
                            pos,
                            sd.TextColor
                            );
                    }
                }
                catch (Exception ex)
                {
                    Program.WriteLog(ex);
                    throw ex;
                }
                finally
                {
                    this.SpriteBatchEnd();
                }
                #endregion
            }
            catch (Exception ex)
            {
                Program.WriteLog(ex);
            }
        }

        private bool IsInsideViewport(Vector2 pos, Vector2 size)
        {
            var camTL = API.Camera.Position;
            var camBR = new Vector2(
                camTL.X + API.UiElements.TileDisplay.Width,
                camTL.Y + API.UiElements.TileDisplay.Height);

            var iTL = pos;
            var iBR = new Vector2(
                iTL.X + size.X,
                iTL.Y + size.Y);

            // Test if right side of item is to the left of camera
            if (iBR.X < camTL.X) { return false; }
            // Test if left side of item is to the right of camera
            if (iTL.X > camBR.X) { return false; }

            // Test of top of item is below camera
            if (iTL.Y > camBR.Y) { return false; }
            // Test if bottom of item is above camera
            if (iBR.Y < camTL.Y) { return false; }

            return true;            
        }

        /// <summary>
        /// Figure out real position of texture, after taking alignment into account
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Vector2 GetRealPosition(IMapItem item)
        {
            Vector2 realPos = item.Position.GetPosition();

            // Adjust horizontal.
            if (item.PositionAlignment.HasFlag(MapItemAlignment.Right))
            {
                // Shift position one texture size to the left,
                // so that the position is located at the right
                // side of the texture.
                realPos.X -= item.Size.X;
            }
            else if (!item.PositionAlignment.HasFlag(MapItemAlignment.Left))
            {
                // Horizontal position should be centered.
                realPos.X -= item.Size.X / 2;
            }

            // Adjust vertical
            if (item.PositionAlignment.HasFlag(MapItemAlignment.Bottom))
            {
                // Shift position one texture size upwards,
                // so that the position is located at the
                // bottom of the texture.
                realPos.Y -= item.Size.Y;
            }
            else if (!item.PositionAlignment.HasFlag(MapItemAlignment.Top))
            {
                // Vertical position should be centered.
                realPos.Y -= item.Size.Y / 2;
            }
            return new Vector2((int)Math.Round(realPos.X), (int)Math.Round(realPos.Y));
        }

        public void DrawTexture(IEnumerable<IMapItem> items, DrawMode drawMode)
        {
            try
            {
                this.SpriteBatchBegin(drawMode);

                foreach (var item in items)
                {
                    if (item.Type != MapItemType.Texture) { continue; }
                    Vector2 realPos = GetRealPosition(item);
                    var tex = item as MapTexture;
                    API.SpriteBatch.Draw(
                        API.Content.Textures.GetTexture(tex.Texture),
                        new Microsoft.Xna.Framework.Rectangle((int)realPos.X, (int)realPos.Y, (int)tex.Size.X, (int)tex.Size.Y),
                        tex.KeyColor);
                }
            }
            catch (Exception ex)
            {
                Program.WriteLog(ex);
            }
            finally
            {
                this.SpriteBatchEnd();
            }
        }

        #region Spritebatch shortcuts
        public bool SpriteBatchBegin(DrawMode mode = DrawMode.World)
        {
            if (API.SpriteBatch == null) { return false; }
            this.SpriteBatchEnd();
            if (API.SpriteBatch.IsDisposed) 
            {
                Program.WriteLog("Starting sprite batch failed: SpriteBatch is disposed.");
                return false; 
            }
            if (mode == DrawMode.ViewPort)
            {
                API.SpriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
                this.IsBatchBegun = true;
                return true;
            }
            API.SpriteBatch.Begin(
                SpriteSortMode.Texture,
                BlendState.AlphaBlend,
                null, null, null, null,
                API.Camera.TransformMatrix);
            this.IsBatchBegun = true;
            return true;
        }

        public void SpriteBatchEnd()
        {
            if (this.IsBatchBegun)
            {
                if (!API.SpriteBatch.IsDisposed)
                {
                    API.SpriteBatch.End();
                }
                this.IsBatchBegun = false;
            }
        }
        #endregion
    }
}
