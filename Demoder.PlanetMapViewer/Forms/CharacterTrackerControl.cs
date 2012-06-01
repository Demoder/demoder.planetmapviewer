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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Demoder.PlanetMapViewer.DataClasses;

namespace Demoder.PlanetMapViewer.Forms
{
    public partial class CharacterTrackerControl : UserControl, ISynchronizeInvoke
    {
        #region members

        private object lockObject = new Object();
        #endregion

        #region Constructors
        public CharacterTrackerControl()
        {
            Context.UiElements.CharacterTrackerControl = this;
            InitializeComponent();

            // Initialize component!
            this.listView1.Dock = DockStyle.Fill;
            this.listView1.View = View.Details;
            this.listView1.CheckBoxes = true;
            
            // Add columns
            this.listView1.Columns.Add("Character", 40, HorizontalAlignment.Center);

            // Add images
            var imgList = new ImageList
            {
                ColorDepth = ColorDepth.Depth32Bit,
                ImageSize = new Size(16, 16),
                TransparentColor = Color.Green,
            };
            imgList.Images.Add("PlayerInShadowlands", Properties.CharacterTrackerControlResources.PlayerInShadowlands);
            imgList.Images.Add("PlayerOnRubika", Properties.CharacterTrackerControlResources.PlayerOnRubika);

            this.listView1.SmallImageList = imgList;
            //this.listView1.StateImageList = imgList;
            this.listView1.LargeImageList = imgList;
            this.listView1.ItemChecked += ItemCheckedChanged;

            // Fix focus stuff!
            this.listView1.GotFocus += ListViewGotFocus;

            this.listView1.ListViewItemSorter = new Demoder.Common.Forms.ListViewSorter();

        }

        #region Handle focus
        void ListViewGotFocus(object sender, EventArgs e)
        {
            this.OnGotFocus(e);
        }
        protected override void OnGotFocus(EventArgs e)
        {
            Context.UiElements.TileDisplay.Focus();
        }
        #endregion

        private void ItemCheckedChanged(object sender, ItemCheckedEventArgs e)
        {
            lock (Context.State.PlayerInfo)
            {
                Context.State.PlayerInfo[(uint)e.Item.Tag].IsTrackedByCamera = e.Item.Checked;
            }
        }
        #endregion

        internal void UpdateCharacterList()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action)delegate() { this.UpdateCharacterList(); });
                return;
            }

            lock (this.lockObject)
            {
                this.listView1.BeginUpdate();
                this.listView1.Items.Clear();

                var playerInfo = Context.State.PlayerInfo.ToArray();
                foreach (var kvp in playerInfo)
                {
                    var pid = kvp.Key;
                    var item = kvp.Value;
                    var li = new ListViewItem();
                    li.Tag = item.ID;

                    // Figure out which text to use.
                    li.Text = item.Name ?? String.Format("N/A (PID: {0}", pid);

                    // Figure out which icon to use.
                    if (item.InShadowlands)
                    {
                        li.ImageKey = "PlayerInShadowlands";
                    }
                    else
                    {
                        li.ImageKey = "PlayerOnRubika";
                    }

                    // Figure out what color to use on the entry.
                    if (!item.IsHooked)
                    {
                        li.ForeColor = SystemColors.GrayText;
                    }                    

                    this.listView1.Items.Add(li);
                }
                Demoder.Common.Forms.AutoResizeHeaders(this.listView1, ColumnHeaderAutoResizeStyle.ColumnContent);
                this.listView1.EndUpdate();
            }
        }

        #region ISynchronizeInvoke
        public new IAsyncResult BeginInvoke(Delegate method, object[] args)
        {
            return this.listView1.BeginInvoke(method, args);
        }

        public new object EndInvoke(IAsyncResult result)
        {
            return this.listView1.EndInvoke(result);
        }

        public new object Invoke(Delegate method, object[] args)
        {
            return this.listView1.Invoke(method, args);
        }

        public new bool InvokeRequired
        {
            get { return this.listView1.InvokeRequired; }
        }
        #endregion
    }
}
