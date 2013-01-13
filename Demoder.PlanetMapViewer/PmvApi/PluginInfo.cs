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
using System.Threading.Tasks;
using System.Threading;
using Demoder.PlanetMapViewer.Helpers;

namespace Demoder.PlanetMapViewer.PmvApi
{
    internal class PluginInfo
    {
        internal PluginInfo()
        {
            this.GenerationMre = new ManualResetEvent(false);
            this.HiddenOverlays = new List<string>();
        }

        public List<string> HiddenOverlays { get; private set; }

        #region Plugin information
        /// <summary>
        /// Whether to display this plugins output
        /// </summary>
        public bool Visible { get; set; }
        public Type Type { get; set; }
        public IPlugin Instance { get; set; }

        internal ManualResetEvent GenerationMre { get; set; }

        /// <summary>
        /// Time between each refresh of CustomMapOverlay
        /// </summary>
        public long RefreshInterval { get; set; }

        /// <summary>
        /// Plugin name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Plugin description
        /// </summary>
        public string Description { get; set; }

        public bool AutoLoad { get; set; }
        #endregion

        #region Runtime information
        /// <summary>
        /// How long time, in milliseconds, did it take to get information from this plugin the last time?
        /// </summary>
        public long LastExecutionTime { get; set;}

        /// <summary>
        /// Task used to generate overlays.
        /// </summary>
        public Task GenerationTask { get; set; }
        
        /// <summary>
        /// The generated overlay
        /// </summary>
        public IEnumerable<MapOverlay> GeneratedOverlay { get; set; }

        public SettingInfo[] Settings { get; set; }
        #endregion
    }
}
