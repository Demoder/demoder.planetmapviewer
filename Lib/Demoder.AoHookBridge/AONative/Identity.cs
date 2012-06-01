/*
* Demoder.AoHookBridge
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
using System.Text;
using System.Runtime.InteropServices;

namespace Demoder.AoHookBridge.AONative
{
    /* Known types:
     * 50000: Character
     * 40016: Playfield
     * insurance terminal: 51005
        mail terminal: 51059
        BS terminal: 51005
       male guard: 50000
      Chirop: 50000

         mission terminal, single: 56001
         mission terminal, team: 56001
         I'm seeing a pattern here
         enter the grid: 51005

        newland/SEB wompah doors in bor:
        ebug: PID: 10956 Message: SetTarget: target: Type: 51016 ID: 3222536992 targeted: False
        ebug: PID: 10956 Message: SetTarget: target: Type: 51016 ID: 3223192352 targeted: False

        Bank: 51005
        all shops: 51035
        Surgery Clinic: 51005
        door: 51016
        Bartender:  Type: 50000 ID: 424330364
     */
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public class Identity
    {
        #region Known types
        /// <summary>
        /// Defines the type for identifying a character
        /// </summary>
        public const uint Character = 50000;
        /// <summary>
        /// Defines the type for identifying a playfield
        /// </summary>
        public const uint Playfield = 40016;
        /// <summary>
        /// Defines the type for identifying a quest/mission
        /// </summary>
        public const uint Quest = 56003;
        #endregion

        public Identity(){}
        public Identity(uint type, uint id)
        {
            this.Type = type;
            this.Instance = id;
        }
        public uint Type { get; set; }
        public uint Instance { get; set; }

        public override string ToString()
        {
            return String.Format("Type: {0} ID: {1}", this.Type, this.Instance);
        }
    }
}
