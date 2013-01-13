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

namespace Demoder.PlanetMapViewer
{
    public enum TutorialStage
    {
        Completed,
        ZoomIn,
        ZoomOut,
        OverlayMode,
        OverlayTitlebarMenuOpen,
        OverlayTitlebarMenuClose,
        OverlayResizeWindow,
        OverlayExit,
        Magnification,
    }

    /// <summary>
    /// Defines alignment of map item.
    /// </summary>
    [Flags]
    public enum MapItemAlignment
    {
        Center = 0,
        Top = 1,
        Bottom = 2,
        Left = 4,
        Right = 8,
    }

    public enum MapItemType
    {
        Unknown = 0,
        Texture = 1,
        SpriteFont = 2,
        TextureWithAttachedSpriteFont=3,
    }

    public enum DrawMode
    {
        World,
        ViewPort
    }

    /// <summary>
    /// Default font mappings
    /// </summary>
    public enum FontType
    {
        GuiSmall,
        GuiNormal,
        GuiLarge,
        GuiXLarge,
#if DEBUG
        CourierNew8,
        CourierNew10,
        CourierNew12,
        CourierNew18,
        Rockwell13,
        Silkscreen7,
#endif
    }

    /// <summary>
    /// Recognized SpriteFonts
    /// </summary>
    public enum LoadedFont
    {
        CourierNew8,
        CourierNew10,
        CourierNew12,
        CourierNew18,

        Rockwell13,

        Silkscreen7,
    }

    public enum CameraControl
    {
        Manual,
        SelectedCharacters,
        ActiveCharacter,
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

    public enum MapType
    {
        Rubika,
        Shadowlands
    }

    public enum DimensionSelection
    {
        AutoDetect=0,
        Atlantean=1,
        Rimor=2,
        Live=4,
        Testlive=9
    }
}
