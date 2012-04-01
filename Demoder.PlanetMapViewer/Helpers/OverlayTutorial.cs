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
    public class OverlayTutorial
    {
        internal Context Context;
        private bool isComplete = false;
        public OverlayTutorial(Context context)
        {
            this.Context = context;
        }

        /// <summary>
        /// Have the user completed the overlay tutorial?
        /// </summary>
        public OverlayTutorialStage CurrentStage
        {
            get
            {
                if (this.isComplete) { return OverlayTutorialStage.Completed; }
                if (Properties.GeneralSettings.Default.DisableTutorials) { return OverlayTutorialStage.Completed; }
                var set = Properties.OverlayTutorial.Default;

                if (!set.TitlebarMenu) { return OverlayTutorialStage.TitlebarMenu; }
                if (!set.ResizeWindow) { return OverlayTutorialStage.ResizeWindow; }
                if (!set.ExitOverlayMode) { return OverlayTutorialStage.ExitOverlayMode; }

                this.isComplete = true;
                return OverlayTutorialStage.Completed;
            }
        }

        public void DrawTutorial()
        {
            switch (this.CurrentStage)
            {
                case OverlayTutorialStage.TitlebarMenu:
                    this.TitlebarMenu();
                    break;
                case OverlayTutorialStage.ResizeWindow:
                    this.ResizeWindow();
                    break;
                case OverlayTutorialStage.ExitOverlayMode:
                    this.ExitOverlayMode();
                    break;
            }
        }

        private void TitlebarMenu()
        {
            var items = new List<IMapItem>();
            int currentHeight = this.Context.UiElements.TileDisplay.Height / 3;
            int center = this.Context.UiElements.TileDisplay.Width / 2;

            {
                var tex = new MapTexture
                {
                    Texture = this.Context.Content.Textures.ArrowUp,
                    PositionAlignment = MapItemAlignment.Top | MapItemAlignment.Center,
                    Size = new Vector2(128, 128),
                    Position = new Vector2(center, 0)
                };
                items.Add(tex);
            }

            items.Add(this.Context.FrameDrawer.GetTutorialStamp(center, currentHeight, 386, 140));

            #region Header
            items.Add(new MapText
            {
                Position = new Vector2(
                    center,
                    currentHeight),
                TextColor = Color.Red,
                Shadow = false,
                Text = "Tutorial: Overlay Menu",
                Font = this.Context.Content.Fonts.GuiXLarge
            });
            currentHeight += (int)items.Last().Size.Y;

            #endregion

            #region Content
            items.Add(new MapText
            {
                Position = new Vector2(
                    center,
                    currentHeight),
                TextColor = Color.White,
                Shadow = false,
                Font = this.Context.Content.Fonts.GuiNormal,
                Text = "The Overlay Menu provides quick access to\r\n" +
                        "many useful controls. You may access it by\r\n" +
                        "right-clicking the title bar.\r\n" +
                        "\r\n" +
                        "Please open the Overlay Menu now."
            });
            currentHeight += (int)items.Last().Size.Y;
            #endregion
            this.Context.FrameDrawer.Draw(items);
        }

        private void ResizeWindow()
        {
            var items = new List<IMapItem>();
            int currentHeight = this.Context.UiElements.TileDisplay.Height / 3;
            int center = this.Context.UiElements.TileDisplay.Width / 2;

            items.Add(this.Context.FrameDrawer.GetTutorialStamp(center, currentHeight, 386, 128));

            #region Header
            items.Add(new MapText
            {
                Position = new Vector2(
                    center,
                    currentHeight),
                TextColor = Color.Red,
                Shadow = false,
                Text = "Tutorial: Window Size",
                Font = this.Context.Content.Fonts.GuiXLarge
            });
            currentHeight += (int)items.Last().Size.Y;
            #endregion

            #region Content
            items.Add(new MapText
            {
                Position = new Vector2(
                    center,
                    currentHeight),
                TextColor = Color.White,
                Shadow = false,
                Font = this.Context.Content.Fonts.GuiNormal,
                Text = "You may resize the window by dragging\r\n" +
                        "the window border using the mouse.\r\n" +
                        "\r\n" +
                        "Please resize the window now."
            });
            currentHeight += (int)items.Last().Size.Y;
            #endregion

            this.Context.FrameDrawer.Draw(items);
        }

        private void ExitOverlayMode()
        {
            var items = new List<IMapItem>();
            int currentHeight = this.Context.UiElements.TileDisplay.Height / 3;
            int center = this.Context.UiElements.TileDisplay.Width / 2;

            items.Add(this.Context.FrameDrawer.GetTutorialStamp(center, currentHeight, 500, 180));

            #region Header
            items.Add(new MapText
            {
                Position = new Vector2(
                    center,
                    currentHeight),
                TextColor = Color.Red,
                Shadow = false,
                Text = "Tutorial: Exiting Overlay Mode",
                Font = this.Context.Content.Fonts.GuiXLarge
            });
            currentHeight += (int)items.Last().Size.Y;
            #endregion

            #region Content
            items.Add(new MapText
            {
                Position = new Vector2(
                    center,
                    currentHeight),
                TextColor = Color.White,
                Shadow = false,
                Font = this.Context.Content.Fonts.GuiNormal,
                Text = "You may exit overlay mode by pressing the close window\r\n"+
                        "button at top right, by opening the Overlay Menu and\r\n"+
                        "selecting 'Exit Overlay Mode', or pressing [F12]\r\n" +
                        "\r\n"+
                        "Please exit overlay mode now."
            });
            currentHeight += (int)items.Last().Size.Y;
            #endregion
            this.Context.FrameDrawer.Draw(items);
        }
    }
}
