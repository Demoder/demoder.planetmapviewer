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
        private TutorialStage setupStage = TutorialStage.Completed;

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
                case TutorialStage.ZoomIn:
                    return this.NormalZoomInTutorial();
                case TutorialStage.ZoomOut:
                    return this.NormalZoomOutTutorial();
                case TutorialStage.OverlayMode:
                    return this.NormalOverlayModeTutorial();
                case TutorialStage.OverlayTitlebarMenu:
                    return this.OverlayTitlebarMenuTutorial();
            }
            return null;
        }

        /// <summary>
        /// Prepares plugin for tutorial steps
        /// </summary>
        /// <param name="stage"></param>
        private void SetupStage(TutorialStage stage)
        {
            this.setupStage = stage;
            switch (stage)
            {
                case TutorialStage.ZoomIn:
                    Context.MapManager.ZoomInEvent += this.FinishZoomInTutorial;
                    break;
                case TutorialStage.ZoomOut:
                    Context.MapManager.ZoomOutEvent += this.FinishZoomOutTutorial;
                    break;
                case TutorialStage.OverlayMode:
                    Context.UiElements.ParentForm.ModeChanged += this.ParentModeChanged;
                    break;
                case TutorialStage.OverlayTitlebarMenu:
                    Context.UiElements.ParentForm.OverlayTitleContextMenuStrip.Closed += this.OverlayMenuFinalizer;
                    break;
            }
        }

        /// <summary>
        /// Cleans up a tutorial.
        /// </summary>
        /// <param name="stage"></param>
        private void CleanupStage(TutorialStage stage)
        {
            switch (stage)
            {
                case TutorialStage.ZoomIn:
                    Context.MapManager.ZoomInEvent -= this.FinishZoomInTutorial;
                    break;
                case TutorialStage.ZoomOut:
                    Context.MapManager.ZoomOutEvent -= this.FinishZoomOutTutorial;
                    break;
                case TutorialStage.OverlayMode:
                    Context.UiElements.ParentForm.ModeChanged -= this.ParentModeChanged;
                    break;
                case TutorialStage.OverlayTitlebarMenu:
                    Context.UiElements.ParentForm.OverlayTitleContextMenuStrip.Closed -= this.OverlayMenuFinalizer;
                    break;
            }
        }

        private TutorialStage CurrentStage
        {
            get
            {
                if (this.isComplete) { return TutorialStage.Completed; }
                
                var norm = Properties.NormalTutorial.Default;
                var over = Properties.OverlayTutorial.Default;
                if (!norm.ZoomIn) { return TutorialStage.ZoomIn; }
                if (!norm.ZoomOut) { return TutorialStage.ZoomOut; }
                if (!norm.OverlayMode) { return TutorialStage.OverlayMode; }
                if (!over.TitlebarMenu) { return TutorialStage.OverlayTitlebarMenu; }
                
                this.isComplete = true;
                return TutorialStage.Completed;
            }
        }

        #region Normal Tutorial steps
        private CustomMapOverlay NormalOverlayModeTutorial()
        {
            var items = new CustomMapOverlay();
            items.MapItems.Add(this.GetTutorialStamp(500, 200));
            var txts = new MapTextBuilder(FontType.GuiNormal, Color.White, Color.Black, true, MapItemAlignment.Top | MapItemAlignment.Left);
            txts.Text("Tutorial: Overlay Mode", textColor: Color.Red, font: FontType.GuiXLarge).Break();
            txts.Text("Overlay Mode maximizes the visible map area and keeps PMV on top of other windows, such as Anarchy Online.", 390).Break();
            txts.Text("You may enter Overlay Mode by going to the top menu and clicking view->Overlay Mode, or by pressing [F12].", 390).Break();
            txts.Break();
            txts.Text("Please enter Overlay Mode now.", textColor: Color.Green).Break();

            items.MapItems.AddRange(txts.ToMapItem(DrawMode.ViewPort, (int)(this.TutorialFramePosition.X - items.MapItems.First().Size.X / 2.5), this.TutorialFramePosition.Y));

            return items;
        }

        private CustomMapOverlay NormalZoomOutTutorial()
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

        private CustomMapOverlay NormalZoomInTutorial()
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

        #region Overlay tutorial steps
        private CustomMapOverlay OverlayTitlebarMenuTutorial()
        {
            if (Context.State.WindowMode != WindowMode.Overlay)
            {
                return this.NormalOverlayModeTutorial();
            }
            var items = new CustomMapOverlay();
            items.MapItems.Add(this.GetTutorialStamp(500, 200));
            int center = Context.UiElements.TileDisplay.Width / 2;

            var tex = new MapTexture
            {
                Texture = Context.Content.Textures.ArrowUp,
                PositionAlignment = MapItemAlignment.Top | MapItemAlignment.Center,
                Size = new Vector2(128, 128),
                Position = new PositionDefinition { Type = DrawMode.ViewPort, X = center, Y = 0 },
            };

            items.MapItems.Add(tex);

            var txts = new MapTextBuilder(FontType.GuiNormal, Color.White, Color.Black, true, MapItemAlignment.Top | MapItemAlignment.Left);
            txts.Text("Tutorial: Overlay Menu", textColor: Color.Red, font: FontType.GuiXLarge).Break();
            txts.Text("The Overlay Menu provides quick access to many useful controls. You may access it by right-clicking the title bar.", 350).Break();
            txts.Break();
            txts.Text("Please open the Overlay Menu now.", textColor: Color.Green).Break();
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
            Properties.NormalTutorial.Default.Save();
        }

        private void FinishZoomOutTutorial()
        {
            Properties.NormalTutorial.Default.ZoomOut = true;
            Properties.NormalTutorial.Default.Save();
        }

        private void ParentModeChanged(WindowMode fromMode, WindowMode toMode)
        {
            if (this.setupStage == TutorialStage.OverlayMode && toMode == WindowMode.Overlay)
            {
                Properties.NormalTutorial.Default.OverlayMode = true;
                Properties.NormalTutorial.Default.Save();
                return;
            }

            if (this.setupStage == TutorialStage.OverlayExit && fromMode == WindowMode.Overlay && toMode != WindowMode.Overlay)
            {
                Properties.OverlayTutorial.Default.ExitOverlayMode = true;
                Properties.OverlayTutorial.Default.Save();
                return;
            }
           
        }

        private void OverlayMenuFinalizer(object sender, System.Windows.Forms.ToolStripDropDownClosedEventArgs e)
        {
            Properties.OverlayTutorial.Default.TitlebarMenu = true;
            Properties.OverlayTutorial.Default.Save();
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
