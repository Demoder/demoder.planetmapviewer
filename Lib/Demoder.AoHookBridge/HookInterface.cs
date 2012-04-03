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
using EasyHook;
using Demoder.AoHookBridge.Events;

namespace Demoder.AoHookBridge
{
    /// <summary>
    /// This will run in the host process and receive calls from the client (hooked process)
    /// </summary>
    public class HookInterface : MarshalByRefObject
    {
        #region Events
        public static event BridgeEventDelegate BridgeEvent;
        #endregion

        private object lockObj = new Object();

        private int processId;

        public void ReportException(Exception ex)
        {
            Console.WriteLine("The target process has reported an error:\r\n{0}", ex);
        }

        public void Ping()
        {
        }

        public void OnIncomingEvents(BridgeEventArgs[] events)
        {
            foreach (var e in events)
            {
                try
                {
                    this.SendBridgeEvent(e);
                }
                catch { }
            }
        }

        /// <summary>
        /// Sends a bridge event
        /// </summary>
        /// <param name="e"></param>
        private void SendBridgeEvent(BridgeEventArgs e)
        {
            var handlers = BridgeEvent;
            if (handlers == null) { return; }
            try
            {
                lock (handlers)
                {
                    handlers(e);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
