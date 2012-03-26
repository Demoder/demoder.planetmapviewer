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

namespace Demoder.PlanetMapViewer.Helpers
{
    public class FrameDrawer
    {
        internal Context Context;
        public FrameDrawer(Context context)
        {
            this.Context = context;
        }

        /// <summary>
        /// Draws text onto the tile display. NOTICE: Starts its own SpriteBatch!
        /// </summary>
        /// <param name="texts"></param>
        /// <param name="worldText">If set to true, positions are relative to map area. If set to false, positions are relative to viewport area.</param>
        public void DrawText(IEnumerable<StringDefinition> texts, bool worldText=false)
        {
            if (this.Context.SpriteBatch == null) { return; }
            try
            {
                #region Shadow
                this.SpriteBatchBegin(worldText);
                try
                {
                    foreach (var sd in texts)
                    {
                        if (sd.Font == null) { continue; }
                        if (!sd.Shadow) { continue; }
                        var textSize = sd.StringMeasure;
                        var fontPos = new Vector2(
                                (float)Math.Floor(sd.CenterPosition.X - (textSize.X / 2)),
                                (float)Math.Floor(sd.CenterPosition.Y + 2)
                                );
                        this.Context.SpriteBatch.DrawString(
                            sd.Font,
                            sd.Text,
                            fontPos,
                            sd.ShadowColor
                            );
                    }
                }
                catch (Exception ex)
                {
                    this.Context.ErrorLog.Enqueue(ex.ToString());
                    throw ex;
                }
                finally
                {
                    this.Context.SpriteBatch.End();
                }
                #endregion

                #region Normal
                this.SpriteBatchBegin(worldText);
                try
                {
                    foreach (var sd in texts)
                    {
                        if (sd.Font == null) { continue; }
                        var textSize = sd.StringMeasure;
                        var fontPos = new Vector2(
                                (float)Math.Floor(sd.CenterPosition.X - textSize.X / 2) - 1,
                                (float)Math.Floor(sd.CenterPosition.Y + 1)
                                );
                        this.Context.SpriteBatch.DrawString(
                            sd.Font,
                            sd.Text,
                            fontPos,
                            sd.TextColor
                            );
                    }
                }
                catch (Exception ex)
                {
                    this.Context.ErrorLog.Enqueue(ex.ToString());
                    throw ex;
                }
                finally
                {
                    this.Context.SpriteBatch.End();
                }
                #endregion
            }
            catch (Exception ex)
            {
                this.Context.ErrorLog.Enqueue(ex.ToString());
            }
        }

        public void TextureCenterOnPixel(Texture2D tex, int x, int y, Microsoft.Xna.Framework.Color color, Vector2 size = default(Vector2))
        {
            try
            {
                if (this.Context.SpriteBatch == null) { return; }
                int width, height;
                if (size != default(Vector2))
                {
                    width = (int)size.X;
                    height = (int)size.Y;
                }
                else
                {
                    width = tex.Width;
                    height = tex.Height;
                }


                this.Context.SpriteBatch.Draw(tex,
                        new Microsoft.Xna.Framework.Rectangle(
                            x - width / 2,
                            y - height / 2,
                            width,
                            height),
                             color);
            }
            catch (Exception ex)
            {
                this.Context.ErrorLog.Enqueue(ex.ToString());
            }
        }

        public void TextureTopMiddleOnPixel(Texture2D tex, int x, int y, Microsoft.Xna.Framework.Color color, Vector2 size = default(Vector2))
        {
            try
            {
                if (this.Context.SpriteBatch == null) { return; }
                int width, height;
                if (size != default(Vector2))
                {
                    width = (int)size.X;
                    height = (int)size.Y;
                }
                else
                {
                    width = tex.Width;
                    height = tex.Height;
                }

                this.Context.SpriteBatch.Draw(tex,
                        new Microsoft.Xna.Framework.Rectangle(
                            x - width / 2,
                            y,
                            width,
                            height),
                             color);
            }
            catch (Exception ex)
            {
                this.Context.ErrorLog.Enqueue(ex.ToString());
            }
        }

        #region Spritebatch shortcuts
        public void SpriteBatchBegin(bool matrixEnabled=true)
        {
            if (!matrixEnabled)
            {
                this.Context.SpriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
                return;
            }
            this.Context.SpriteBatch.Begin(
                SpriteSortMode.Texture,
                BlendState.AlphaBlend,
                null, null, null, null,
                this.Context.Camera.TransformMatrix);
        }
        #endregion
    }
}
