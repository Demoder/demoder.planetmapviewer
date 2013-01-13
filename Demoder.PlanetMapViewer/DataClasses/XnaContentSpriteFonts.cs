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
using System.IO;

namespace Demoder.PlanetMapViewer.DataClasses
{
    public class XnaContentSpriteFonts
    {
        #region Members
        private Dictionary<LoadedFont, SpriteFont> fonts = new Dictionary<LoadedFont, SpriteFont>();

        #endregion

        public void Load()
        {
            foreach (var file in (new DirectoryInfo(@"Content\Fonts")).GetFiles("*.xnb", SearchOption.TopDirectoryOnly))
            {
                LoadedFont fontType;
                var contentName = file.Name.Substring(0, file.Name.Length - 4);
                if (!Enum.TryParse<LoadedFont>(contentName, true, out fontType))
                {
                    continue;
                }
                this.fonts[fontType] = API.ContentManager.Load<SpriteFont>(@"Fonts\" + contentName);
            }
        }

        public SpriteFont GetFont(LoadedFont type)
        {
            return this.fonts[type];
        }

        public LoadedFont GetLoadedFont(FontType type)
        {
            try
            {
                var name = type.ToString();
                if (name.StartsWith("Gui"))
                {
                    name = name.Remove(0, 3);
                    return (LoadedFont)Properties.GuiFonts.Default[name];
                }
                else if (name.StartsWith("Map"))
                {
                    name = name.Remove(0, 3);
                    return (LoadedFont)Properties.MapFonts.Default[name];
                }
                else
                {
                    return (LoadedFont)Enum.Parse(typeof(LoadedFont), name);
                }
            }
            catch(Exception ex)
            {
                Program.WriteLog(ex);
                return LoadedFont.Rockwell13;
            }
        }

        public void SetLoadedFont(FontType type, LoadedFont font)
        {
            var name = type.ToString();
            if (name.StartsWith("Gui"))
            {
                name = name.Remove(0, 3);
                Properties.GuiFonts.Default[name] = font;
            }
            else if (name.StartsWith("Map"))
            {
                name = name.Remove(0, 3);
                Properties.MapFonts.Default[name] = font;
            }
        }

        public SpriteFont GetFont(FontType type)
        {
            return this.GetFont(this.GetLoadedFont(type));
        }
    }     
}
