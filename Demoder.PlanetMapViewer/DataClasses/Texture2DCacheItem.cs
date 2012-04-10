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
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace Demoder.PlanetMapViewer.DataClasses
{
    public class Texture2DCacheItem : IDisposable
    {
        #region Members
        private Texture2D texture;
        private Stopwatch lastAccess;
        #endregion

        #region Accessors
        public long LastAccess { get { return this.lastAccess.ElapsedMilliseconds; } }
        public Texture2D Texture { get { return this; } }
        public bool IsDisposed { get; private set; }
        #endregion

        public Texture2DCacheItem(Texture2D texture)
        {
            this.texture = texture;
            this.lastAccess = Stopwatch.StartNew();
        }

        #region Operators
        public static implicit operator Texture2D(Texture2DCacheItem ctx)
        {
            ctx.lastAccess.Restart();
            return ctx.texture;
        }

        public static implicit operator Texture2DCacheItem(Texture2D tex)
        {
            return new Texture2DCacheItem(tex);
        }
        #endregion

        public void Dispose()
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException("Object already disposed!", new Exception());
            }
            this.lastAccess.Reset();
            this.texture.Dispose();
            this.IsDisposed = true;
        }
    }
}
