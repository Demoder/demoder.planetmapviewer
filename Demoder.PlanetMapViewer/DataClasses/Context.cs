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
using System.Windows.Forms;
using Demoder.PlanetMapViewer.Forms;
using Demoder.PlanetMapViewer.Helpers;
using Demoder.PlanetMapViewer.Xna;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Demoder.PlanetMapViewer.DataClasses
{
    public class Context
    {
        public Context()
        {
            this.UiElements = new ContextUiElements();
            this.Options = new ContextOptions();
            this.Content = new XnaContent();
            this.FrameDrawer = new FrameDrawer(this);
            this.Tutorial = new Tutorial(this);
        }

        public Queue<string> ErrorLog = new Queue<string>();

        public FrameDrawer FrameDrawer { get; private set; }
        public MapManager MapManager;
        public Camera Camera;
        public SpriteBatch SpriteBatch;
        public HookInfoTracker HookInfo;
        public ContentManager ContentManager;
        public XnaContent Content { get; private set; }

        public Tutorial Tutorial { get; private set; }

        public GraphicsDevice GraphicsDevice
        {
            get
            {
                if (this.UiElements.TileDisplay == null) { return null; }
                return this.UiElements.TileDisplay.GraphicsDevice;
            }
        }

        public ContextUiElements UiElements { get; private set; }
        public ContextOptions Options { get; private set; }
    }

    public class ContextUiElements
    {
        public MainWindow ParentForm;
        public VScrollBar VScrollBar;
        public HScrollBar HScrollBar;
        public ComboBox MapList;
        public TileDisplay TileDisplay;
    }

    public class ContextOptions
    {
        public bool IsMapRubika = true;
        public CameraControl CameraControl = CameraControl.Character;
        public bool IsOverlayMode = false;
    }

    public enum CameraControl
    {
        Manual,
        Character
    }
}
