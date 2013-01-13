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
using System.Runtime.Remoting;
using System.Text;
using EasyHook;

namespace Demoder.AoHookBridge
{
    /// <summary>
    /// Manages injection hooks
    /// </summary>
    public static class Injector
    {
        /// <summary>
        /// Injects enabled hooks to the desired process id
        /// </summary>
        public static string Inject(int processId, BridgeEventType enabledHooks = (BridgeEventType)uint.MaxValue)
        {
            string channelName = null;
            Console.WriteLine("Injecting to {0}!", processId);
            try
            {
                // Register assemblies to the GAC
                Config.Register(
                    "AOHook library by Demoder",
                    "Demoder.AoHookBridge.dll"
                    );
                
                RemoteHooking.IpcCreateServer<HookInterface>(
                    ref channelName,
                    WellKnownObjectMode.SingleCall);

                RemoteHooking.Inject(
                    processId,
                    "Demoder.AoHookBridge.dll",
                    "Demoder.AoHookBridge.dll",
                    channelName,
                    enabledHooks,
                    processId);


                return channelName;
            }
            catch(Exception ex) 
            {
                Console.WriteLine("Injector::Inject: Exception:");
                Console.WriteLine(ex.ToString());
            }
            return null;
        }
    }
}
