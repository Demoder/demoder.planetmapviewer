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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Demoder.PlanetMapViewer.Forms
{
    public partial class CameraUserControl : UserControl
    {
        public CameraUserControl()
        {
            InitializeComponent();
        }

        private void zoomInButton_Click(object sender, EventArgs e)
        {
            API.UiElements.TileDisplay.ZoomIn();
            API.UiElements.TileDisplay.Focus();
        }

        private void zoomOutButton_Click(object sender, EventArgs e)
        {
            API.UiElements.TileDisplay.ZoomOut();
            API.UiElements.TileDisplay.Focus();
        }

        private void characterButton_Click(object sender, EventArgs e)
        {
            API.State.CameraControl = CameraControl.SelectedCharacters;
            API.UiElements.TileDisplay.Focus();
        }

        private void activeCharacterButton_Click(object sender, EventArgs e)
        {
            API.State.CameraControl = CameraControl.ActiveCharacter;
            API.UiElements.TileDisplay.Focus();
        }
    }
}
