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

            var tex = new MapTexture(this.Context)
            {
                Texture = this.Context.Content.Textures.ArrowUp,
                PositionAlignment = MapItemAlignment.Top | MapItemAlignment.Center,
                Size = new Vector2(128, 128),
                Position = new PositionDefinition { Type = DrawMode.ViewPort, X = center, Y = 0 },
            };
                items.Add(tex);
            items.Add(this.Context.FrameDrawer.GetTutorialStamp(center, currentHeight, 386, 140));

            // Add texts.
            var txts = new MapTextBuilder(this.Context, FontType.GuiNormal, Color.White, Color.Black, true, MapItemAlignment.Top);
            txts.Text("Tutorial: Overlay Menu", textColor: Color.Red, font: FontType.GuiXLarge).Break();
            txts.Text("The Overlay Menu provides quick access to many useful controls. You may access it by right-clicking the title bar.", 350).Break();
            txts.Break();
            txts.Text("Please open the Overlay Menu now.").Break();

            items.AddRange(txts.ToMapItem(DrawMode.ViewPort, this.Context.UiElements.TileDisplay.Width / 2, this.Context.UiElements.TileDisplay.Height / 3));

            this.Context.FrameDrawer.Draw(items);
        }

        private void ResizeWindow()
        {
            var items = new List<IMapItem>();
            int currentHeight = this.Context.UiElements.TileDisplay.Height / 3;
            int center = this.Context.UiElements.TileDisplay.Width / 2;

            items.Add(this.Context.FrameDrawer.GetTutorialStamp(center, currentHeight, 386, 128));

            // Add texts.
            var txts = new MapTextBuilder(this.Context, FontType.GuiNormal, Color.White, Color.Black, true, MapItemAlignment.Top);
            txts.Text("Tutorial: Window Size", textColor: Color.Red, font: FontType.GuiXLarge).Break();
            txts.Text("You may resize the window by dragging the window border using the mouse.", 350).Break();
            txts.Break();
            txts.Text("Please resize the window now.").Break();
            items.AddRange(txts.ToMapItem(DrawMode.ViewPort, this.Context.UiElements.TileDisplay.Width / 2, this.Context.UiElements.TileDisplay.Height / 3));

            this.Context.FrameDrawer.Draw(items);
        }

        private void ExitOverlayMode()
        {
            var items = new List<IMapItem>();
            int currentHeight = this.Context.UiElements.TileDisplay.Height / 3;
            int center = this.Context.UiElements.TileDisplay.Width / 2;

            items.Add(this.Context.FrameDrawer.GetTutorialStamp(center, currentHeight, 500, 150));

            // Add texts.
            var txts = new MapTextBuilder(this.Context, FontType.GuiNormal, Color.White, Color.Black, true, MapItemAlignment.Top);
            txts.Text("Tutorial: Exiting Overlay Mode", textColor: Color.Red, font: FontType.GuiXLarge).Break();
            txts.Text("You may exit overlay mode by pressing the close window button at top right, by opening the Overlay Menu and selecting 'Exit Overlay Mode', or pressing [F12]", 480).Break();
            txts.Break();
            txts.Text("Please exit overlay mode now.", textColor: Color.Green).Break();
            items.AddRange(txts.ToMapItem(DrawMode.ViewPort, this.Context.UiElements.TileDisplay.Width / 2, this.Context.UiElements.TileDisplay.Height / 3));

            this.Context.FrameDrawer.Draw(items);
        }
    }
}
