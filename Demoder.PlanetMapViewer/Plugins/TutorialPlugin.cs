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
using Demoder.PlanetMapViewer.PmvApi;
using Demoder.PlanetMapViewer.DataClasses;
using Microsoft.Xna.Framework;
using Demoder.PlanetMapViewer.Helpers;

namespace Demoder.PlanetMapViewer.Plugins
{
    public class TutorialPlugin : IPlugin
    {
        private bool isComplete = false;
        /// <summary>
        /// Which stage is set up now.
        /// </summary>
        private NormalTutorialStage setupStage = NormalTutorialStage.Completed;

        public CustomMapOverlay GetCustomOverlay()
        {
            if (Properties.GeneralSettings.Default.DisableTutorials) { return null; }
            var stage = this.CurrentStage;
            if (this.setupStage != stage)
            {
                this.CleanupStage(this.setupStage);
                this.SetupStage(stage);
            }
            switch (this.CurrentStage)
            {
                case NormalTutorialStage.ZoomIn:
                    return this.ZoomInTutorial();
                case NormalTutorialStage.ZoomOut:
                    return this.ZoomOutTutorial();
                case NormalTutorialStage.OverlayMode:
                    return this.OverlayModeTutorial();
            }
            return null;
        }

        /// <summary>
        /// Prepares plugin for tutorial steps
        /// </summary>
        /// <param name="stage"></param>
        private void SetupStage(NormalTutorialStage stage)
        {
            this.setupStage = stage;
            switch (stage)
            {
                case NormalTutorialStage.ZoomIn:
                    Context.MapManager.ZoomInEvent += this.FinishZoomInTutorial;
                    break;
                case NormalTutorialStage.ZoomOut:
                    Context.MapManager.ZoomOutEvent += this.FinishZoomOutTutorial;
                    break;
            }

        }

        /// <summary>
        /// Cleans up a tutorial.
        /// </summary>
        /// <param name="stage"></param>
        private void CleanupStage(NormalTutorialStage stage)
        {
            switch (stage)
            {
                case NormalTutorialStage.ZoomIn:
                    Context.MapManager.ZoomInEvent -= this.FinishZoomInTutorial;
                    break;
                case NormalTutorialStage.ZoomOut:
                    Context.MapManager.ZoomOutEvent -= this.FinishZoomOutTutorial;
                    break;
            }
        }

        private NormalTutorialStage CurrentStage
        {
            get
            {
                if (this.isComplete) { return NormalTutorialStage.Completed; }
                
                var set = Properties.NormalTutorial.Default;
                if (!set.ZoomIn) { return NormalTutorialStage.ZoomIn; }
                if (!set.ZoomOut) { return NormalTutorialStage.ZoomOut; }
                if (!set.OverlayMode) { return NormalTutorialStage.OverlayMode; }
                
                this.isComplete = true;
                return NormalTutorialStage.Completed;
            }
        }

        #region Tutorial steps
        private CustomMapOverlay OverlayModeTutorial()
        {
            return null;
        }

        private CustomMapOverlay ZoomOutTutorial()
        {
            var items = new CustomMapOverlay();
            items.MapItems.Add(this.GetTutorialStamp(500, 200));
            var txts = new MapTextBuilder(FontType.GuiNormal, Color.White, Color.Black, true, MapItemAlignment.Top | MapItemAlignment.Left);
            txts.Text("Tutorial: Zooming Out", textColor: Color.Red, font: FontType.GuiXLarge).Break();
            txts.Text("You may zoom out on the map by the following means:").Break();
            txts.Text("- Double right-click anywhere on the map").Break();
            txts.Text("- Pressing the 'Zoom Out' button to the right").Break();
            txts.Text("- Pressing the - key on your keyboard").Break();
            txts.Text("- Using your keyboards zoom button, if it has one").Break();
            txts.Break();
            txts.Text("Please zoom out now.", textColor: Color.Green).Break();

            items.MapItems.AddRange(txts.ToMapItem(DrawMode.ViewPort, (int)(this.TutorialFramePosition.X - items.MapItems.First().Size.X / 2.5), this.TutorialFramePosition.Y));

            return items;
        }

        private CustomMapOverlay ZoomInTutorial()
        {
            var items = new CustomMapOverlay();
            items.MapItems.Add(this.GetTutorialStamp(500, 200));
            var txts = new MapTextBuilder(FontType.GuiNormal, Color.White, Color.Black, true, MapItemAlignment.Top| MapItemAlignment.Left);
            txts.Text("Tutorial: Zooming in", textColor: Color.Red, font: FontType.GuiXLarge).Break();
            txts.Text("You may zoom in on the map by the following means:").Break();
            txts.Text("- Double left-click anywhere on the map").Break();
            txts.Text("- Pressing the 'Zoom In' button to the right").Break();
            txts.Text("- Pressing the + key on your keyboard").Break();
            txts.Text("- Using your keyboards zoom button, if it has one").Break();
            txts.Break();
            txts.Text("Please zoom in now.", textColor: Color.Green).Break();

            items.MapItems.AddRange(txts.ToMapItem(DrawMode.ViewPort, (int)(this.TutorialFramePosition.X - items.MapItems.First().Size.X / 2.5), this.TutorialFramePosition.Y));

            return items;
        }
        #endregion

        private Point TutorialFramePosition
        {
            get
            {
                return new Point(Context.UiElements.TileDisplay.Width / 2, Context.UiElements.TileDisplay.Height / 3);
            }
        }

        #region Tutorial finalizers
        private void FinishZoomInTutorial()
        {
            Properties.NormalTutorial.Default.ZoomIn = true;
        }

        private void FinishZoomOutTutorial()
        {
            Properties.NormalTutorial.Default.ZoomOut = true;
        }
        #endregion

        public MapTexture GetTutorialStamp(int width, int height)
        {
            int posX = Context.UiElements.TileDisplay.Width / 2;
            int posY = Context.UiElements.TileDisplay.Height / 3;
            

            var tex = new MapTexture
            {
                Texture = Context.Content.Textures.TutorialFrame,
                Position = new PositionDefinition { Type = DrawMode.ViewPort, X = posX, Y = posY - 15 },
                Size = new Vector2(width, height),
                PositionAlignment = MapItemAlignment.Top
            };

            return tex;
        }
    }
}
