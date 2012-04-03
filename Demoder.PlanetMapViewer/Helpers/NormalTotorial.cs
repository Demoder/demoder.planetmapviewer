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
    public class NormalTutorial
    {
        internal Context Context;
        private bool isComplete = false;

        public NormalTutorial(Context context)
        {
            this.Context = context;
        }

        /// <summary>
        /// Have the user completed the overlay tutorial?
        /// </summary>
        public NormalTutorialStage CurrentStage
        {
            get
            {
                if (this.isComplete) { return NormalTutorialStage.Completed; }
                if (Properties.GeneralSettings.Default.DisableTutorials) { return NormalTutorialStage.Completed; }

                var set = Properties.NormalTutorial.Default;
                if (!set.ZoomIn) { return NormalTutorialStage.ZoomIn; }
                if (!set.ZoomOut) { return NormalTutorialStage.ZoomOut; }
                if (!set.OverlayMode) { return NormalTutorialStage.OverlayMode; }
                this.isComplete = true;
                return NormalTutorialStage.Completed;
            }
        }

        public void DrawTutorial()
        {
            switch (this.CurrentStage)
            {
                case NormalTutorialStage.ZoomIn:
                    this.ZoomIn();
                    break;
                case NormalTutorialStage.ZoomOut:
                    this.ZoomOut();
                    break;
                case NormalTutorialStage.OverlayMode:
                    this.OverlayMode();
                    break;
            }
        }

        private void ZoomIn()
        {
            var items = new List<IMapItem>();
            int currentHeight = this.Context.UiElements.TileDisplay.Height / 3;
            int center = this.Context.UiElements.TileDisplay.Width / 2;

            items.Add(this.Context.FrameDrawer.GetTutorialStamp(center, currentHeight, 500, 200));

            #region Header
            items.Add(new MapText
            {
                Position = new Vector2(
                    center,
                    currentHeight),
                PositionAlignment = MapItemAlignment.Top,
                TextColor = Color.Red,
                Shadow = false,
                Text = "Tutorial: Zooming In",
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
                PositionAlignment = MapItemAlignment.Top,
                TextColor = Color.White,
                Shadow = false,
                Font = this.Context.Content.Fonts.GuiNormal,
                Text = "You may zoom in on the map by the following means:\r\n" +
                "- Double left-click anywhere on the map\r\n" +
                "- Pressing the 'Zoom In' button to the right\r\n" +
                "- Pressing the + key on your keyboard\r\n" +
                "- Using your keyboards zoom button, if it has one\r\n" +
                "\r\n" +
                "Please zoom in now."
            });
            currentHeight += (int)items.Last().Size.Y;
            #endregion
            this.Context.FrameDrawer.Draw(items, DrawMode.ViewPort);
        }

        private void ZoomOut()
        {
            var items = new List<IMapItem>();
            int currentHeight = this.Context.UiElements.TileDisplay.Height / 3;
            int center = this.Context.UiElements.TileDisplay.Width / 2;

            items.Add(this.Context.FrameDrawer.GetTutorialStamp(center, currentHeight, 500, 200));
            
            #region Header
            items.Add(new MapText
            {
                Position = new Vector2(
                    center,
                    currentHeight),
                PositionAlignment = MapItemAlignment.Top,
                TextColor = Color.Red,
                Shadow = false,
                Text = "Tutorial: Zooming Out",
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
                PositionAlignment = MapItemAlignment.Top,
                TextColor = Color.White,
                Shadow = false,
                Font = this.Context.Content.Fonts.GuiNormal,
                Text = "You may zoom out on the map by the following means:\r\n" +
                "- Double right-click anywhere on the map\r\n" +
                "- Pressing the 'Zoom Out' button to the right\r\n" +
                "- Pressing the - key on your keyboard\r\n" +
                "- Pressing [Ctrl] plus your keyboards zoom button,\r\n" +
                "  if it has one\r\n" +
                "\r\n" +
                "Please zoom out now."
            });
            currentHeight += (int)items.Last().Size.Y;
            #endregion
            this.Context.FrameDrawer.Draw(items, DrawMode.ViewPort);
        }

        private void OverlayMode()
        {
            var items = new List<IMapItem>();
            int currentHeight = this.Context.UiElements.TileDisplay.Height / 3;
            int center = this.Context.UiElements.TileDisplay.Width / 2;

            items.Add(this.Context.FrameDrawer.GetTutorialStamp(center, currentHeight, 425, 210));

            #region Header
            items.Add(new MapText
            {
                Position = new Vector2(
                    center,
                    currentHeight),
                PositionAlignment = MapItemAlignment.Top,
                TextColor = Color.Red,
                Shadow = false,
                Text = "Tutorial: Overlay Mode",
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
                PositionAlignment = MapItemAlignment.Top,
                TextColor = Color.White,
                Shadow = false,
                Font = this.Context.Content.Fonts.GuiNormal,
                Text =  "Overlay Mode maximizes the visible map area\r\n" +
                        "and keeps PMV on top of other windows, such\r\n" +
                        "as Anarchy Online.\r\n" +
                        "\r\n" +
                        "You may enter Overlay Mode by going to\r\n"+
                        "the top menu and clicking view->Overlay Mode,\r\n" +
                        "or by pressing [F12].\r\n" +
                        "\r\n" + 
                        "Please enter overlay mode now."
            });
            currentHeight += (int)items.Last().Size.Y;
            #endregion

            this.Context.FrameDrawer.Draw(items, DrawMode.ViewPort);
        }        
    }
}
