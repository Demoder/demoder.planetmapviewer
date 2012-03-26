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
using Microsoft.Xna.Framework;

namespace Demoder.PlanetMapViewer.Helpers
{
    public class Tutorial
    {
        public Tutorial(Context context)
        {
            this.Overlay = new OverlayTutorial(context);
        }

        public OverlayTutorial Overlay;
    }

    public class OverlayTutorial
    {
        internal Context Context;
        public OverlayTutorial(Context context)
        {
            this.Context = context;
        }

        /// <summary>
        /// Have the user completed the overlay tutorial?
        /// </summary>
        public bool Completed
        {
            get
            {
                var set = Properties.OverlayTutorial.Default;

                if (!set.TitlebarMenu) { return false; }

                return true;
            }
        }

        public void DrawTutorial()
        {
            var set = Properties.OverlayTutorial.Default;
            if (!set.TitlebarMenu)
            {
                this.DrawTitlebarMenu();
                return;
            }
        }

        private void DrawTitlebarMenu()
        {

            var texts = new List<StringDefinition>();
            int currentHeight = this.Context.UiElements.TileDisplay.Height / 3;
            int center = this.Context.UiElements.TileDisplay.Width / 2;
            #region Header
            texts.Add(new StringDefinition
            {
                CenterPosition = new Vector2(
                    center,
                    currentHeight),
                TextColor = Color.Red,
                Shadow = false,
                Text = "Tutorial",
                Font = this.Context.Content.Fonts.GuiXLarge
            });
            currentHeight += (int)texts.Last().StringMeasure.Y;

            this.Context.FrameDrawer.DrawText(texts, false);
            texts.Clear();
            #endregion

            #region Content
            texts.Add(new StringDefinition
            {
                CenterPosition = new Vector2(
                    center,
                    currentHeight),
                TextColor = Color.White,
                Shadow = false,
                Font = this.Context.Content.Fonts.GuiNormal,
                Text = "Please right-click the title bar now to\r\n" +
                        "open the Overlay Mode menu, which provides\r\n" +
                        "quick access to many useful controls."
            });
            currentHeight += (int)texts.Last().StringMeasure.Y;

            this.Context.FrameDrawer.DrawText(texts, false);

            this.Context.FrameDrawer.SpriteBatchBegin(false);
            this.Context.FrameDrawer.TextureTopMiddleOnPixel(this.Context.Content.Textures.ArrowUp, center, 0, Color.White, new Vector2(64,64));
            this.Context.SpriteBatch.End();


            #endregion

        }
    }
}
