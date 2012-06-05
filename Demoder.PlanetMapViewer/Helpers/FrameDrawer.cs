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
                        var textSize = sd.Size;
                        var pos = GetRealPosition(item);
                        pos.X++;
                        pos.Y++;
                        API.SpriteBatch.DrawString(
                            API.Content.Fonts.GetFont(sd.Font),
                            sd.Text,
                            pos,
                            sd.ShadowColor
                            );
                    }
                }
                catch (Exception ex)
                {
                    Program.WriteLog(ex.ToString());
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
                        var textSize = sd.Size;
                        var pos = GetRealPosition(item);                       

                        API.SpriteBatch.DrawString(
                            API.Content.Fonts.GetFont(sd.Font),
                            sd.Text,
                            pos,
                            sd.TextColor
                            );
                    }
                }
                catch (Exception ex)
                {
                    Program.WriteLog(ex.ToString());
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
                Program.WriteLog(ex.ToString());
            }
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
            if (item.PositionAlignment.HasFlag(MapItemAlignment.Left))
            {
               // don't touch anything
            }
            else if (item.PositionAlignment.HasFlag(MapItemAlignment.Right))
            {
                // Shift position one texture size to the left,
                // so that the position is located at the right
                // side of the texture.
                realPos.X -= item.Size.X;
            }
            else
            {
                // Horizontal position should be centered.
                realPos.X -= item.Size.X / 2;
            }

            // Adjust vertical
            if (item.PositionAlignment.HasFlag(MapItemAlignment.Top))
            {
                // Don't do anything
            }
            else if (item.PositionAlignment.HasFlag(MapItemAlignment.Bottom))
            {
                // Shift position one texture size upwards,
                // so that the position is located at the
                // bottom of the texture.
                realPos.Y -= item.Size.Y;
            }
            else
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
                Program.WriteLog(ex.ToString());
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
                API.SpriteBatch.End();
                this.IsBatchBegun = false;
            }
        }
        #endregion
    }
}
