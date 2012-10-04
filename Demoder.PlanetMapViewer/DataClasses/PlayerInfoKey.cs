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
using Demoder.Common.AO;

namespace Demoder.PlanetMapViewer.DataClasses
{
    public class PlayerInfoKey
    {
        public PlayerInfoKey(uint serverID, uint characterID)
        {
            this.Dimension = GetDimension(serverID);
            this.CharacterID = characterID;
        }
        public Dimension Dimension { get; set; }
        public uint CharacterID { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is PlayerInfoKey)
            {
                return this == (PlayerInfoKey)obj;
            }
            else
            {
                return this == obj;
            }
        }

        public override int GetHashCode()
        {
            return (int)this.CharacterID;
        }

        public static bool operator ==(PlayerInfoKey one, PlayerInfoKey two)
        {
            if (Object.ReferenceEquals(one, two)) { return true; }
            if (Object.ReferenceEquals(one, null)) { return false; }
            if (Object.ReferenceEquals(two, null)) { return false; }

            if (one.CharacterID != two.CharacterID) { return false; }
            if (one.Dimension != two.Dimension) { return false; }
            return true;
        }

        public static bool operator !=(PlayerInfoKey one, PlayerInfoKey two)
        {
            return !(one == two);
        }

        public override string ToString()
        {
            return String.Format("{0} {1}", this.Dimension, this.CharacterID);
        }

        internal static Dimension GetDimension(uint serverID)
        {
            if (serverID >= 3000 && serverID <= 3099) { return Dimension.Atlantean; }
            if (serverID >= 3100 && serverID <= 3199) { return Dimension.Rimor; }
            if (serverID >= 3900 && serverID <= 3999) { return Dimension.Testlive; }
            else { return Dimension.Unknown; }
        }
    }
}
