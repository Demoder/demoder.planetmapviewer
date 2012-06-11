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
using Demoder.AoHook;
using Demoder.PlanetMapViewer.PmvApi;
using Demoder.PlanetMapViewer.DataClasses;
using System.IO;
using Demoder.Common.Cache;

namespace Demoder.PlanetMapViewer
{
    public static class API
    {
        static API()
        {
            PluginManager = new PluginManager();
            UiElements = new ContextUiElements();
            State = new ContextState();
            Content = new XnaContent();
            FrameDrawer = new FrameDrawer();
            PluginConfig = new PluginConfig(
                new DirectoryInfo(
                    Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        "Demoder.PlanetMapViewer")
                        )
                    );

            XmlCache = new XmlCacheWrapper(
                new DirectoryInfo(
                    Path.Combine(
                        Demoder.Common.Misc.MyTemporaryDirectory,
                        "XmlCache")));
        }

        public static FrameDrawer FrameDrawer { get; private set; }
        public static MapManager MapManager;
        public static Camera Camera;
        public static SpriteBatch SpriteBatch;
        public static HookInfoTracker AoHook { get; internal set; }
        public static ContentManager ContentManager;
        public static XnaContent Content { get; private set; }

        public static XmlCacheWrapper XmlCache { get; private set; }

        public static GraphicsDevice GraphicsDevice
        {
            get
            {
                if (UiElements.TileDisplay == null) { return null; }
                return UiElements.TileDisplay.GraphicsDevice;
            }
        }

        public static ContextUiElements UiElements { get; private set; }
        public static ContextState State { get; private set; }

        public static PluginManager PluginManager { get; private set; }

        public static PluginConfig PluginConfig { get; private set; }
    }
}
