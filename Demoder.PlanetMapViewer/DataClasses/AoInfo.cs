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
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Demoder.PlanetMapViewer.DataClasses
{
    public class AoInfo
    {
        /// <summary>
        /// The process ID we're monitoring
        /// </summary>
        public int ProcessID { get; private set; }

        public PlayerInfo Character { get; private set; }
        public QuestInfo Mission { get; private set; }

        /// <summary>
        /// Time since last mofification to information in this data structure
        /// </summary>
        public Stopwatch LastModified { get; private set; }


        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, this)) { return true; }
            if (obj is AoInfo)
            {
                var blah = obj as AoInfo;
                if (blah.ProcessID == this.ProcessID)
                {
                    return true;
                }
            }
            return false;
        }

        public AoInfo(int processID)
        {
            this.ProcessID = processID;
            this.Character = new PlayerInfo();
            this.Character.Position = Vector3.Zero;
            this.Character.Zone = new ZoneInfo();
            this.LastModified = Stopwatch.StartNew();
            this.Mission = new QuestInfo();
        }

        public override string ToString()
        {
            if (this.Character != null)
            {
                if (String.IsNullOrWhiteSpace(this.Character.Name) && this.Character.ID != uint.MinValue && this.Character.ID != uint.MaxValue)
                {
                    return "cid:"+this.Character.ID.ToString();
                }
                if (!String.IsNullOrWhiteSpace(this.Character.Name))
                {
                    return this.Character.Name;
                }
            }
            return "pid:" + this.ProcessID.ToString();
        }
    }

    public class PlayerInfo
    {
        public uint ID;
        public string Name;
        public ZoneInfo Zone = new ZoneInfo();
        public Vector3 Position = new Vector3();
        public bool InShadowlands = false;
    }

    public class QuestInfo
    {
        public uint ID;
        public ZoneInfo Zone = new ZoneInfo();
        public Vector3 ZonePosition = new Vector3();
        public Vector3 WorldPosition = new Vector3();
    }

    public class ZoneInfo
    {
        public uint ID { get; set; }
        public string Name { get; set; }
    }
}
