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
using Demoder.PlanetMapViewer.DataClasses;
using Microsoft.Xna.Framework;

namespace Demoder.PlanetMapViewer.PmvApi
{
    public class GuiTableOverlay : GuiOverlay
    {
        private Dictionary<int, Dictionary<int, IMapItem>> items = new Dictionary<int, Dictionary<int, IMapItem>>();
     
        /// <summary>
        /// Should header be printed? default to true.
        /// </summary>
        public bool HeaderVisible { get; set; }

        /// <summary>
        /// Table headers
        /// </summary>
        public GuiTableHeaders Headers { get; private set; }
        
        /// <summary>
        /// Table position
        /// </summary>
        public Point Position { get; set; }

        public IMapItem Title { get; set; }

        /// <summary>
        /// Pixels between each column. Defaults is 8.
        /// </summary>
        public int ColumnSpacing { get; set; }

        /// <summary>
        /// Pixels between each row. Default is 1.
        /// </summary>
        public int RowSpacing { get; set; }

        public GuiTableOverlay()
            : base()
        {
            this.Headers = new GuiTableHeaders();
            this.HeaderVisible = true;
            this.ColumnSpacing = 8;
            this.RowSpacing = 1;
        }

        public GuiOverlay ToGuiOverlay()
        {
            var overlay = new GuiOverlay
            {

            };
            this.AutoSizeHeaders();
            this.AdjustPositions();

            this.Title.Position.X = this.Position.X;
            this.Title.Position.Y = this.Position.Y;
            this.Title.Position.Zone = 0;

            // Add items
            if (this.Title != null)
            {
                overlay.MapItems.Add(this.Title);
            }
            if (this.HeaderVisible)
            {
                // Add headers
                overlay.MapItems.AddRange(this.Headers.ToMapItems());
            }
            // Add rows
            foreach (var row in this.items)
            {
                overlay.MapItems.AddRange(row.Value.Values);
            }

            return overlay;            
        }

        private void AutoSizeHeaders()
        {
            if (this.Headers.ToArray().Count(h => h.AutoSize) == 0) { return; }
            foreach (var row in this.items)
            {
                foreach (var column in row.Value)
                {
                    var header = this.Headers[column.Key];
                    if (!header.AutoSize) { continue; }
                    if (column.Value.Size.X > header.Width)
                    {
                        header.Width = (int)column.Value.Size.X;
                    }
                }
            }
        }

        private Point FirstRowPosition
        {
            get
            {
                if (this.Title != null)
                {
                    return new Point(
                        (int)(this.Position.X),
                        (int)(this.Position.Y + this.Title.Size.Y + (this.RowSpacing * 2)));
                }
                return this.Position;
            }
        }

        private void AdjustPositions()
        {
            var yOffset = this.FirstRowPosition.Y;

            // Adjust headers
            if (this.HeaderVisible)
            {
                var largestHeight = 0;
                var xOffset = this.FirstRowPosition.X;
                foreach (var header in this.Headers.ToArray())
                {
                    header.Content.Position.X = xOffset;
                    header.Content.Position.Y = yOffset;

                    if (header.Content.PositionAlignment.HasFlag(MapItemAlignment.Left))
                    {

                    }
                    else if (header.Content.PositionAlignment.HasFlag(MapItemAlignment.Right))
                    {
                        header.Content.Position.X += header.Width;
                    }
                    else
                    {
                        header.Content.Position.X += header.Width / 2;
                    }

                    header.Content.Position.Type = DrawMode.ViewPort;

                    if (header.Content.Size.Y > largestHeight)
                    {
                        largestHeight = (int)header.Content.Size.Y;
                    }
                    xOffset += header.Width + this.ColumnSpacing;
                }
                yOffset += largestHeight + (this.RowSpacing * 2);
            }

            // Adjust rows
            foreach (var row in this.items)
            {
                var xOffset = this.FirstRowPosition.X;
                var largestHeight = 0;

                foreach (var column in row.Value)
                {
                    var columnHeader = this.Headers[column.Key];
                    // Record largest height for this row.
                    if (column.Value.Size.Y > largestHeight)
                    {
                        largestHeight = (int)column.Value.Size.Y;
                    }

                    column.Value.Position.X = xOffset;
                    column.Value.Position.Y = yOffset;
                    column.Value.Position.Type = DrawMode.ViewPort;
                    column.Value.PositionAlignment = columnHeader.Alignment;

                    if (columnHeader.Alignment.HasFlag(MapItemAlignment.Left))
                    {

                    }
                    else if (columnHeader.Alignment.HasFlag(MapItemAlignment.Right))
                    {
                        column.Value.Position.X += columnHeader.Width;
                    }
                    else
                    {
                        column.Value.Position.X += columnHeader.Width / 2;
                    }

                    // Increment position by column header width.
                    xOffset += columnHeader.Width + this.ColumnSpacing;
                }
                yOffset += largestHeight + this.RowSpacing;
            }
        }

        public IMapItem this[int row, int column]
        {
            get 
            {
                if (!this.items.ContainsKey(row))
                {
                    return null;
                }
                if (!this.items[row].ContainsKey(column))
                {
                    return null;
                }
                return this.items[row][column];
            }
            set 
            {
                if (!this.items.ContainsKey(row))
                {
                    this.items[row] = new Dictionary<int, IMapItem>();
                }
                this.items[row][column] = value;
            }
        }
    }

    public class GuiTableHeaders
    {
        private Dictionary<int, GuiTableHeader> headers = new Dictionary<int, GuiTableHeader>();

        public GuiTableHeader this[int column]
        {
            get
            {
                if (!this.headers.ContainsKey(column))
                {
                    // Generate default column
                    this.headers[column] = new GuiTableHeader
                    {
                        Width = 50,
                        Content = new MapText
                        {
                            Font = API.Content.Fonts.GetFont(LoadedFont.Silkscreen7),
                            TextColor = Color.White,
                            OutlineColor = Color.Black,
                            Text = "Column" + column.ToString()
                        }
                    };
                }
                
                return this.headers[column];
            }
            set
            {
                this.headers[column] = value;
            }
        }

        public IMapItem[] ToMapItems()
        {
            // TODO: Clone the items instead of referencing them.
            /*var items = new List<IMapItem>();
            foreach (var item in this.headers.Values)
            {
                items.Add(item.Clone());
            }*/
            return this.headers.Values.Select(i=>i.Content).ToArray();
        }

        public GuiTableHeader[] ToArray()
        {
            // TODO: Clone the items instead of referencing them.
            /*var items = new List<IMapItem>();
            foreach (var item in this.headers.Values)
            {
                items.Add(item.Clone());
            }*/
            return this.headers.Values.ToArray();
        }
    }

    public class GuiTableHeader
    {
        public IMapItem Content { get; set; }
        public int Width { get; set; }
        public bool AutoSize { get; set; }
        public MapItemAlignment Alignment { get; set; }
    }
}
