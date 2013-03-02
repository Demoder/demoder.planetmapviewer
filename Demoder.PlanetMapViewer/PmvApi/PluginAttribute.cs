/*
* Demoder.PlanetMapViewer
* Copyright (C) 2012, 2013 Demoder (demoder@demoder.me)
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

namespace Demoder.PlanetMapViewer.PmvApi
{
    public class PluginAttribute : Attribute
    {
        /// <summary>
        /// Interval between re-drawing of this plugins CustomMapOverlay
        /// </summary>
        public long RefreshInterval { get;private set; }

        /// <summary>
        /// Name of this plugin
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Defines a plugin name, and sets refresh interval to once per frame.
        /// </summary>
        /// <param name="name">Plugin name</param>
        public PluginAttribute(string name)
            : this(name, -1)
        {}

        /// <summary>
        /// Sets a custom refresh interval.<br/> 
        /// Use this if the data isn't expected to change for every frame.
        /// </summary>
        /// <param name="name">Plugin name</param>
        /// <param name="interval">Seconds between each refresh.</param>
        public PluginAttribute(string name, double interval)
        {
            this.RefreshInterval = (long)(interval * 1000);
            this.Name = name;

            if (this.RefreshInterval < 5)
            {
                this.RefreshInterval = -1;
            }
        }
    }
}
