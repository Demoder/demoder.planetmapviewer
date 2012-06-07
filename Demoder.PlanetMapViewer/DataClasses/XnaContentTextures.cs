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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading;
using Demoder.Common.Cache;
using Demoder.Common.Extensions;
using Demoder.PlanetMapViewer.Forms;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;

namespace Demoder.PlanetMapViewer.DataClasses
{
    public class XnaContentTextures
    {
        #region Members
        internal const long MemoryCacheTime = 60000;
        internal const long DiskCacheDays = 7;
        private const string textureSite = "http://static.aodevs.com/";
        private WebClient iconWebClient;
        private WebClient guiWebClient;
        private Timer CleanTimer;
        

        private FileCache iconCache;
        private FileCache guiCache;

        /// <summary>
        /// Contains our bundled textures
        /// </summary>
        private Dictionary<string, Texture2DCacheItem> contentTextures = new Dictionary<string, Texture2DCacheItem>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// Contains AO GUI textures
        /// </summary>
        private Dictionary<string, Texture2DCacheItem> guiTextures = new Dictionary<string, Texture2DCacheItem>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// Contains AO icon textures
        /// </summary>
        private Dictionary<string, Texture2DCacheItem> iconTextures = new Dictionary<string, Texture2DCacheItem>(StringComparer.InvariantCultureIgnoreCase);
        #endregion

        #region Constructors
        public XnaContentTextures()
        {
            this.CleanTimer = new Timer(this.RemoveExpiredItems, null, 30000, 30000);
            this.iconCache = new FileCache(new DirectoryInfo(Path.Combine(Demoder.Common.Misc.MyTemporaryDirectory, "IconCache")));
            this.guiCache = new FileCache(new DirectoryInfo(Path.Combine(Demoder.Common.Misc.MyTemporaryDirectory, "GuiCache")));
        }

        private void SetupIconWebClient()
        {
            this.iconWebClient = new WebClient();
            this.iconWebClient.Headers.Set(HttpRequestHeader.UserAgent, Assembly.GetAssembly(typeof(XnaContentTextures)).GetName().Name);
            this.iconWebClient.DownloadDataCompleted += this.iconCache.WebClientDownloadDataCompleted;
        }

        private void SetupGuiWebClient()
        {
            this.guiWebClient = new WebClient();
            this.guiWebClient.Headers.Set(HttpRequestHeader.UserAgent, Assembly.GetAssembly(typeof(XnaContentTextures)).GetName().Name);
            this.guiWebClient.DownloadDataCompleted += this.guiCache.WebClientDownloadDataCompleted;
        }

        #endregion

        #region Cleanup tasks
        private void RemoveExpiredItems(object obj)
        {
            this.RemoveExpiredItems(this.contentTextures);
            this.RemoveExpiredItems(this.guiTextures);
            this.RemoveExpiredItems(this.iconTextures);
        }
        private void RemoveExpiredItems(Dictionary<string, Texture2DCacheItem> cache)
        {
            lock (cache)
            {
                foreach (var element in contentTextures.Where(kvp => kvp.Value.LastAccess > MemoryCacheTime).Select(kvp => kvp.Key).ToArray())
                {
                    cache[element].Dispose();
                    cache.Remove(element);
                }
            }
        }
        #endregion

        public Texture2D GetTexture(TextureDefinition definition)
        {
            return this.GetTexture(definition.Type, definition.Key);
        }

        public Texture2D GetTexture(TextureType type, string key)
        {
            switch (type)
            {
                case TextureType.Gui:
                    return this.GetAoGuiTexture(key);
                case TextureType.Icon:
                    return this.GetAoIconTexture(key);
                case TextureType.Content:
                    return this.GetContentTexture(key);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Downloads a texture from static.aodevs.com
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private Texture2D GetAoIconTexture(string key)
        {
            Texture2DCacheItem texCache;
            lock (this.iconTextures)
            {
                // Is it already loaded?
                if (this.iconTextures.TryGetValue(key, out texCache))
                {
                    return texCache;
                }

                // Load it!
                try
                {
                    byte[] data;
                    if ((DateTime.Now - this.iconCache.Time(key)).Days > DiskCacheDays)
                    {

                        data = this.DownloadAoIconTexture(key);
                    }
                    else
                    {
                        data = this.iconCache.Read(key);
                    }
                    var ms = new MemoryStream(data);
                    var tex = Texture2D.FromStream(API.GraphicsDevice, ms);
                    ms.Dispose();

                    texCache = tex;
                    this.iconTextures[key] = texCache;
                }
                catch
                {
                    if (!key.Equals("ErrorTexture", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return this.GetContentTexture("ErrorTexture");
                    }
                }
            }
            return texCache;
        }

        internal void DownloadAoIconTextures(bool force, params string[] keys)
        {           
            foreach (var key in keys)
            {
                if (force || (DateTime.Now - this.iconCache.Time(key)).Days > 7)
                {
                    this.DownloadAoIconTexture(key);
                }
            }
        }

        internal byte[] DownloadAoIconTexture(string key)
        {
            lock (this)
            {
                if (this.iconWebClient == null) { this.SetupIconWebClient(); }
            }
            var data = this.iconWebClient.WaitForReady().DownloadData(new Uri(String.Format("{0}icon/{1}", textureSite, key)));
            if (data == null) { throw new Exception(); }
            this.iconCache.Cache(key, data);

            return data;
        }

        /// <summary>
        /// Loads a texture from the default AO GUI
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private Texture2D GetAoGuiTexture(string key)
        {
            Texture2DCacheItem texCache;

            lock (this.guiTextures)
            {
                // Is it already loaded?
                if (this.guiTextures.TryGetValue(key, out texCache))
                {
                    return texCache;
                }

                // Load it!
                try
                {
                    byte[] data;
                    if ((DateTime.Now - this.guiCache.Time(key)).Days > DiskCacheDays)
                    {
                        data = this.DownloadAoGuiTexture(key);
                    }
                    else
                    {
                        data = this.guiCache.Read(key);
                    }
                    
                    var ms = new MemoryStream(data);
                    var tex = Texture2D.FromStream(API.GraphicsDevice, ms);
                    ms.Dispose();

                    texCache = tex;
                    this.iconTextures[key] = texCache;
                }
                catch
                {
                    if (!key.Equals("ErrorTexture", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return this.GetContentTexture("ErrorTexture");
                    }
                }
            }
            return texCache;
        }

        private byte[] DownloadAoGuiTexture(string key)
        {
            lock (this)
            {
                if (this.guiWebClient == null) { this.SetupGuiWebClient(); }
            }
            var data = this.guiWebClient.WaitForReady().DownloadData(new Uri(String.Format("{0}texture/gui/{1}", textureSite, key)));
            if (data == null) { throw new Exception(); }
            this.guiCache.Cache(key, data);

            return data;
        }



        private Texture2D GetContentTexture(string key)
        {
            Texture2DCacheItem tex;
            lock (this.contentTextures)
            {
                if (this.contentTextures.TryGetValue(key, out tex))
                {
                    return tex;
                }
                try
                {
                    var tex2 = API.ContentManager.Load<Texture2D>(String.Format(@"Textures\{0}", key));
                    tex = tex2;
                    this.contentTextures[key] = tex;
                }
                catch
                {
                    if (!key.Equals("ErrorTexture", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return this.GetContentTexture("ErrorTexture");
                    }
                }
                return tex;
            }
        }

        public TextureDefinition CharacterLocator { get { return new TextureDefinition(TextureType.Content, "CharacterMarker"); } }
        public TextureDefinition MissionLocator { get { return new TextureDefinition(TextureType.Content, "MissionMarker"); } }
        public TextureDefinition ArrowUp { get { return new TextureDefinition(TextureType.Content, "ArrowUp"); } }
        public TextureDefinition TutorialFrame { get { return new TextureDefinition(TextureType.Content, "TutorialFrame"); } }
    }

    public enum TextureType
    {
        /// <summary>
        /// Bundled with application
        /// </summary>
        Content,
        /// <summary>
        /// AO GUI texture
        /// </summary>
        Gui,
        /// <summary>
        /// AO Icon Texture
        /// </summary>
        Icon
    }
}
