﻿/*
* Demoder.AoHook
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
using Demoder.AoHook.Events;

namespace Demoder.AoHook
{
    public delegate void DebugEventDelegate(Provider sender, DebugEventArgs e);
    public delegate void CharacterPositionEventDelegate(Provider sender, CharacterPositionEventArgs e);
    public delegate void HookStateChangeEventDelegate(Provider sender, HookStateChangeEventArgs e);
    public delegate void DynelNameEventDelegate(Provider sender, DynelNameEventArgs e);
    public delegate void QuestLocationEventDelegate(Provider sender, QuestLocationEventArgs e);
    public delegate void ServerIdEventDelegate(Provider sender, ServerIdEventArgs e);

    public delegate void CharacterLoginEventDelegate(Provider sender, CharacterLoginEventArgs e);
    public delegate void CharacterLogoutEventDelegate(Provider sender, CharacterLogoutEventArgs e);
}
