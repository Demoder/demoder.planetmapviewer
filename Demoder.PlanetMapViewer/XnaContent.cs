using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Demoder.PlanetMapViewer
{
    public class XnaContent
    {
        public XnaContent()
        {
            this.Textures = new XnaContentTextures();
            this.Fonts = new XnaContentSpriteFonts();
        }
        public XnaContentTextures Textures { get; private set; }
        public XnaContentSpriteFonts Fonts { get; private set; }
    }

    public class XnaContentTextures
    {
        public Texture2D CharacterLocator;
    }

    public class XnaContentSpriteFonts
    {
        public SpriteFont CharacterName;
    }
}
