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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Demoder.PlanetMapViewer.Helpers
{
    /// <summary>
    /// Helps create formatted text
    /// </summary>
    public class MapTextBuilder
    {
        #region Members
        private List<IMapItem> mapItems = new List<IMapItem>();
        private SpriteFont font;
        private Color textColor;
        private Color shadowColor;
        private bool haveShadow;

        private Vector2 nextPosition = Vector2.Zero;
        private MapItemAlignment alignment;

        private bool didBreakOnLastTextAlready = false;

        /// <summary>
        /// How many milliseconds before an entry times out. 0 means never.
        /// </summary>
        public uint MessagesTimeoutAfter = 0;
        #endregion

        public MapTextBuilder(FontType font, Color textColor, Color shadowColor, bool haveShadow, MapItemAlignment alignment)
            : this(API.Content.Fonts.GetFont(font), textColor, shadowColor, haveShadow, alignment)
        {   
        }

        public MapTextBuilder(LoadedFont font, Color textColor, Color shadowColor, bool haveShadow, MapItemAlignment alignment)
            : this(API.Content.Fonts.GetFont(font), textColor, shadowColor, haveShadow, alignment)
        {
        }

        public MapTextBuilder(SpriteFont font, Color textColor, Color shadowColor, bool haveShadow, MapItemAlignment alignment)
        {
            this.font = font;
            this.textColor = textColor;
            this.shadowColor = shadowColor;
            this.haveShadow = haveShadow;
            this.alignment = alignment;
        }

        /// <summary>
        /// Insert a linebreak
        /// </summary>
        /// <returns></returns>
        public MapTextBuilder Break(int breaks=0)
        {
            this.nextPosition.X = 0;
            if (!this.didBreakOnLastTextAlready)
            {
                this.nextPosition.Y += this.mapItems.Last().Size.Y;
            }
            else if (breaks==0)
            {
                breaks++;
            }

            if (breaks > 0)
            {
                this.nextPosition.Y += (breaks * this.font.MeasureString("S").Y);
            }
            this.didBreakOnLastTextAlready = true;
            return this;
        }

        #region Colors
        public MapTextBuilder TextColor(Color color)
        {
            this.textColor = color;
            return this;
        }

        public MapTextBuilder ShadowColor(Color color)
        {
            this.shadowColor = color;
            return this;
        }

        public MapTextBuilder Color(Color textColor, Color shadowColor)
        {
            this.textColor = textColor;
            this.shadowColor = shadowColor;
            return this;
        }
        #endregion
        
        public MapTextBuilder Text(string text)
        {
            var txt = new MapText
            {
                Font = this.font,
                Outline = this.haveShadow,
                TextColor = this.textColor,
                OutlineColor = this.shadowColor,
                Position = new PositionDefinition(this.nextPosition.X, this.nextPosition.Y),
                PositionAlignment = this.alignment,
                Text = text
            };
            
            this.mapItems.Add(txt);
            this.nextPosition.X += txt.Size.X;
            this.didBreakOnLastTextAlready = false;
            return this;
        }
        
        /// <summary>
        /// Insert linebreaks to make text fit provided width
        /// </summary>
        /// <param name="text"></param>
        /// <param name="breakWidth"></param>
        /// <returns></returns>
        public MapTextBuilder Text(string text, int breakWidth)
        {

            var testItem = new MapText
            {
                Font = this.font,
                Outline = this.haveShadow,
                TextColor = this.textColor,
                OutlineColor = this.shadowColor,
                Position = new PositionDefinition(this.nextPosition.X, this.nextPosition.Y),
                PositionAlignment = this.alignment,
                Text = text
            };

            var words = testItem.Text.Split(" ".ToArray(), StringSplitOptions.None);
            var lastAcceptedText = words[0];
            for (int i = 1; i < words.Length; i++ )
            {
                testItem.Text = lastAcceptedText + " " + words[i];
                if (testItem.Size.X + this.nextPosition.X > breakWidth)
                {

                    if (!String.IsNullOrEmpty(lastAcceptedText))
                    {
                        if (this.nextPosition.X != 0)
                        {
                            this.Text(lastAcceptedText).Break();
                            lastAcceptedText = "";
                            continue;
                        }
                        testItem.Text = lastAcceptedText + "\n" + words[i];
                    }
                }
                lastAcceptedText = testItem.Text;             
            }
            this.Text(lastAcceptedText);
            return this;
        }

        public MapTextBuilder Text(string text, FontType font, Color? textColor = null, Color? shadowColor = null, bool? haveShadow = null, MapItemAlignment? alignment = null)
        {
            this.Text(text,
                textColor,
                shadowColor,
                API.Content.Fonts.GetFont(font),
                haveShadow,
                alignment);
            return this;
        }

        public MapTextBuilder Text(string text, LoadedFont font, Color? textColor = null, Color? shadowColor = null, bool? haveShadow = null, MapItemAlignment? alignment = null)
        {
            this.Text(text, 
                textColor, 
                shadowColor, 
                API.Content.Fonts.GetFont(font), 
                haveShadow, 
                alignment);
            return this;
        }

        public MapTextBuilder Text(string text, Color? textColor = null, Color? shadowColor = null, SpriteFont font = null, bool? haveShadow = null, MapItemAlignment? alignment = null)
        {
            // Store old values
            var oldTxtColor = this.textColor;
            var oldShadowColor = this.shadowColor;
            var oldFont = this.font;
            var oldHaveShadow = this.haveShadow;
            var oldAlignment = this.alignment;

            // Set temporary values
            if (textColor.HasValue) { this.textColor = textColor.Value; }
            if (shadowColor.HasValue) { this.shadowColor = shadowColor.Value; }
            if (font != null) { this.font = font; }
            if (haveShadow.HasValue) { this.haveShadow = haveShadow.Value; }
            if (alignment.HasValue) { this.alignment = alignment.Value; }

            // Add the text
            this.Text(text);

            // Restore old values
            this.textColor = oldTxtColor;
            this.shadowColor = oldShadowColor;
            this.font = oldFont;
            this.haveShadow = oldHaveShadow;
            this.alignment = oldAlignment;

            return this;
        }


        public IMapItem[] ToMapItem(DrawMode mode, int x, int y)
        {
            var items = new IMapItem[this.mapItems.Count];

            for (int i = 0; i < items.Length; i++)
            {
              
                var item = this.mapItems[i].Clone() as MapText;
                item.Position = new PositionDefinition(item.Position.X + x, item.Position.Y + y);
                item.Position.Type = mode;
                items[i]=item;
            }
            return items.ToArray();
        }

        public void Draw(DrawMode mode, int x, int y)
        {
            var items = this.ToMapItem(mode, x, y);
            API.FrameDrawer.Draw(items);
        }
    }
}
