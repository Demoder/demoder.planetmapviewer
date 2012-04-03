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
using Microsoft.Xna.Framework.Graphics;

namespace Demoder.PlanetMapViewer.DataClasses
{
    public class XnaContent
    {
        public XnaContent()
        {
            this.Textures = new XnaContentTextures();
            this.Fonts = new XnaContentSpriteFonts();
        }

        public bool Loaded = false;
        public XnaContentTextures Textures { get; private set; }
        public XnaContentSpriteFonts Fonts { get; private set; }
    }

    public class XnaContentTextures
    {
        public Texture2D CharacterLocator;
        public Texture2D MissionLocator;
        public Texture2D ArrowUp;
        public Texture2D TutorialFrame;
    }

    public class XnaContentSpriteFonts
    {
        public SpriteFont CharacterName;
        public SpriteFont GuiSmall;
        public SpriteFont GuiNormal;
        public SpriteFont GuiLarge;
        public SpriteFont GuiXLarge;

        public SpriteFont MapSmall;
    }
}
