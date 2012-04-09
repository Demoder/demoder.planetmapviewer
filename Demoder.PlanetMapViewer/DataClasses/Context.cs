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
using System.Windows.Forms;
using Demoder.PlanetMapViewer.Forms;
using Demoder.PlanetMapViewer.Helpers;
using Demoder.PlanetMapViewer.Xna;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Demoder.PlanetMapViewer.DataClasses
{
    public class Context
    {
        public Context()
        {
            this.UiElements = new ContextUiElements();
            this.State = new ContextState();
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
        public ContextState State { get; private set; }
    }

    public class ContextUiElements
    {
        public MainWindow ParentForm;
        public VScrollBar VScrollBar;
        public HScrollBar HScrollBar;
        public ComboBox MapList;
        public TileDisplay TileDisplay;
        public CharacterTrackerControl CharacterTrackerControl;
    }

    public class ContextState
    {
        /// <summary>
        /// Which map type is currently selected, if any?
        /// </summary>
        public MapType MapType = MapType.Rubika;
        /// <summary>
        /// Is autoswitching between map types enabled?
        /// </summary>
        public bool MapTypeAutoSwitching = true;
        public CameraControl CameraControl = CameraControl.Character;
        /// <summary>
        /// Current window mode
        /// </summary>
        public WindowMode WindowMode = WindowMode.Normal;

        /// <summary>
        /// Percent magnification. 2 = twice size, 0.5=half size.
        /// </summary>
        public float Magnification = 1;

        public Dictionary<uint, PlayerInfo> PlayerInfo = new Dictionary<uint, PlayerInfo>();

        public List<TimedMapText> GuiNotifications = new List<TimedMapText>();
    }


    public enum CameraControl
    {
        Manual,
        Character,
    }

    /// <summary>
    /// Which mode is the application in?
    /// </summary>
    public enum WindowMode
    {
        Normal,
        Fullscreen,
        Overlay
    }
}
