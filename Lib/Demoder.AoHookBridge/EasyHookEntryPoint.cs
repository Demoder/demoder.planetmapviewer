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
using Demoder.AoHookBridge;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using Demoder.AoHookBridge.AONative;
using Demoder.AoHookBridge.Events;

namespace Demoder.AoHookBridge
{
    /// <summary>
    /// This will run inside the client (hooked process). Should make calls to HookInterface.
    /// </summary>
    public class EasyHookEntryPoint : IEntryPoint
    {

        internal Queue<string> debugQueue = new Queue<string>();
        /// <summary>
        /// Interface to report to.
        /// </summary>
        private HookInterface hookInterface;

        public DataStore DataStore = new DataStore();

        private bool aborted = false;

        #region Hooks
        private LocalHook aoHookFrameProcess;
        private LocalHook aoHookGetQuestWorldPos;
        #endregion

        private Queue<BridgeEventArgs> eventsQueue = new Queue<BridgeEventArgs>();
        private BridgeEventType enabledHooks;

        public int ProcessID { get; private set; }

        private Stopwatch hookTimer;

        #region Easyhook and bridge pushing
        /// <summary>
        /// Spawned within the client (hooked process)
        /// </summary>
        /// <param name="context"></param>
        /// <param name="channelName"></param>
        public EasyHookEntryPoint(RemoteHooking.IContext context, string channelName, BridgeEventType enabledHooks, int processId)
        {
            // Retrieve a reference to our interface
            this.hookInterface = RemoteHooking.IpcConnectClient<HookInterface>(channelName);
            // Validate connection
            this.hookInterface.Ping();
            this.enabledHooks = enabledHooks;
            this.ProcessID = processId;
        }

        /// <summary>
        /// Installs hooks into AO, and shuffles data across the Alpha Bridge.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="channelName"></param>
        public void Run(RemoteHooking.IContext context, string channelName, BridgeEventType enabledHooks, int processId)
        {
            this.enabledHooks = enabledHooks;
            try
            {
                #region Install hooks
                // FrameProcess
                this.aoHookFrameProcess = LocalHook.Create(
                    LocalHook.GetProcAddress("Interfaces.dll", "?FrameProcess@Client_t@@QAEXXZ"),
                    new API.Interfaces.Client_t.AoFrameProcessDelegate(Hooks.Interfaces.Client_t.AoFrameProcess),
                    this
                    );
                this.aoHookFrameProcess.ThreadACL.SetExclusiveACL(new int[] { 0 });

                // Retrieve quest/mission locator information when uploaded to map
                this.aoHookGetQuestWorldPos = LocalHook.Create(LocalHook.GetProcAddress("Interfaces.dll", "?N3Msg_GetQuestWorldPos@N3InterfaceModule_t@@QBE_NABVIdentity_t@@AAV2@AAVVector3_t@@2@Z"),
                    new API.Interfaces.N3InterfaceModule.GetQuestWorldPosDelegate(Hooks.Interfaces.N3InterfaceModule.GetQuestWorldPos),
                    this
                    );
                this.aoHookGetQuestWorldPos.ThreadACL.SetExclusiveACL(new int[] { 0 });
                #endregion
            }
            catch (Exception ex)
            {
                this.hookInterface.ReportException(ex);
                return;
            }

            // Notify that we've successfully installed a hook. Pass along character ID if available.
            this.SendBridgeEvent(new HookStateChangeEventArgs(true));


            #region Push messages across Alpha Bridge
            try
            {
                this.hookTimer = Stopwatch.StartNew();
                while (!this.aborted)
                {
                    bool sendPing = true;
                    try
                    {
                        sendPing = this.ProcessEventQueue();
                    }
                    catch (Exception ex)
                    {
                        this.hookInterface.ReportException(ex);
                    }
                    if (sendPing)
                    {
                        this.hookInterface.Ping();
                    }
                    // Limit to 30 updates per second.
                    Thread.Sleep(33);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    this.hookInterface.ReportException(ex);
                }
                catch { /* Bridge is dead; Can't report exception. */ }
            }
            #endregion

            #region Uninstall hooks
            try
            {
                // Uninstall hooks
                if (this.aoHookFrameProcess != null)
                {
                    this.aoHookFrameProcess.Dispose();
                }
                if (this.aoHookGetQuestWorldPos != null)
                {
                    this.aoHookGetQuestWorldPos.Dispose();
                }

            }
            catch { }

            try
            {
                this.SendBridgeEvent(new HookStateChangeEventArgs(false));
                this.ProcessEventQueue();
            }
            catch { }
            #endregion
        }

        private bool ProcessEventQueue()
        {
            BridgeEventArgs[] events;
            lock (this.eventsQueue)
            {
                if (this.eventsQueue.Count == 0) { return false; }
                events = this.eventsQueue.ToArray();
                this.eventsQueue.Clear();
            }
            this.hookInterface.OnIncomingEvents(events);
            return true;
        }
        #endregion

        #region Internal methods
        internal void SendBridgeEvent(BridgeEventArgs e)
        {
            try
            {
                lock (this.eventsQueue)
                {
                    this.eventsQueue.Enqueue(e);
                }
            }
            catch { }
        }
        internal void Debug(bool report, string message, params object[] parms)
        {
            if (!report) { return; }
            this.SendBridgeEvent(new DebugEventArgs(
                String.Format(message, parms)
                ));
        }
        #endregion
    }
}
