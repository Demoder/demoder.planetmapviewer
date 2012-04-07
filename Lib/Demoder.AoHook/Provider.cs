/*
* Demoder.AoHook
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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using hEvents = Demoder.AoHook.Events;
using Demoder.AoHookBridge;
using bEvents = Demoder.AoHookBridge.Events;
using Demoder.Common;

namespace Demoder.AoHook
{
    public class Provider
    {
        #region Private members
        private Thread aoProcessPollerThread;
        /// <summary>
        /// Stores information about processes we've successfully hooked
        /// </summary>
        private Dictionary<int, Process> successfulHooks = new Dictionary<int, Process>();
        /// <summary>
        /// Stores information about processes we've failed to hook
        /// </summary>
        private Dictionary<int, Process> failedHooks = new Dictionary<int, Process>();


        private QueueHelper<hEvents.HookEventArgs> outgoingEvents;

        private Dictionary<int, PersistentInfo> persistentInfo = new Dictionary<int, PersistentInfo>();
        #endregion

        #region Events
        public event DebugEventDelegate DebugEvent;
        public event CharacterPositionEventDelegate CharacterPositionEvent;
        public event HookStateChangeEventDelegate HookStateChangeEvent;
        public event DynelNameEventDelegate DynelNameEvent;
        public event QuestLocationEventDelegate QuestLocationEvent;

        #endregion
        /// <summary>
        /// Creates a default instance.
        /// </summary>
        public Provider()
        {
            this.outgoingEvents = new QueueHelper<hEvents.HookEventArgs>(this.SendEvents);
            this.aoProcessPollerThread = new Thread(this.AoProcessPoller);
            this.aoProcessPollerThread.IsBackground = true;
            this.aoProcessPollerThread.Name = "Provider->AoProcessPoller()";
            HookInterface.BridgeEvent += new BridgeEventDelegate(this.ProcessBridgeEvents);
        }
        
        /// <summary>
        /// Hook into AO
        /// </summary>
        public void HookAo()
        {
            this.aoProcessPollerThread.Start();
        }

        /// <summary>
        /// Looks for AO processes to hook into.
        /// </summary>
        private void AoProcessPoller()
        {
            while (true)
            {
                // Find candidates
                var candidates = from process in Process.GetProcesses()
                                 where process.ProcessName.Equals("client", StringComparison.InvariantCultureIgnoreCase) || process.ProcessName.Equals("AnarchyOnline", StringComparison.InvariantCultureIgnoreCase)
                                 where !this.successfulHooks.ContainsKey(process.Id)    // Exclude already hooked processes
                                 where !this.failedHooks.ContainsKey(process.Id)        // Exclude processes we've failed to hook
                                 select process;
                foreach (var process in candidates)
                {
                    try
                    {
                        Injector.Inject(process.Id);
                        this.successfulHooks.Add(process.Id, process);
                    }
                    catch
                    {
                        this.failedHooks.Add(process.Id, process);
                    }
                }

                // Clean out old successful hooks
                var releasedHooks = (from process in this.successfulHooks
                    where process.Value.HasExited
                    select process.Key).ToArray();
                foreach (var pid in releasedHooks)
                {
                    this.SendEvents(new hEvents.HookStateChangeEventArgs(pid, false));
                    this.successfulHooks.Remove(pid);
                }

                // Clean out old failed hooks
                releasedHooks = (from process in this.failedHooks
                                where process.Value.HasExited
                                select process.Key).ToArray();
                foreach (var pid in releasedHooks)
                {
                    Console.WriteLine("Non-hooked process have exited: {0}", pid);
                    this.failedHooks.Remove(pid);
                }

                Thread.Sleep(5000);
            }
        }

        private void ProcessBridgeEvents(bEvents.BridgeEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Type: {0}", e.EventType);
            Console.ForegroundColor = ConsoleColor.White;
            switch (e.EventType)
            {
                case BridgeEventType.CharacterPosition:
                    this.ProcessCharacterPositionEvent(e as bEvents.DynelPositionEventArgs);
                    break;
                case BridgeEventType.DynelName:
                    this.outgoingEvents.Enqueue(new hEvents.DynelNameEventArgs(e as bEvents.DynelNameEventArgs));
                    break;
                case BridgeEventType.Debug:
                    this.outgoingEvents.Enqueue(new hEvents.DebugEventArgs(e as bEvents.DebugEventArgs));
                    break;
                case BridgeEventType.HookStateChange:
                    this.outgoingEvents.Enqueue(new hEvents.HookStateChangeEventArgs(e as bEvents.HookStateChangeEventArgs));
                    break;
                case BridgeEventType.QuestLocation:
                    this.outgoingEvents.Enqueue(new hEvents.QuestLocationEventArgs(e as bEvents.QuestLocationEventArgs));
                    break;
            }
        }

        /// <summary>
        /// Key: Process ID
        /// </summary>
        private Dictionary<int, long[]> currentCharacterPositions = new Dictionary<int, long[]>();

        private bEvents.DynelPositionEventArgs curCharacterPos = null;
        private void ProcessCharacterPositionEvent(bEvents.DynelPositionEventArgs e)
        {
            if (this.curCharacterPos != null)
            {
                if (this.curCharacterPos.Equals(e)) { return; }
            }
            //Console.WriteLine("N3: X: {0} Y: {1} Z: {2}", e.X, e.Y, e.Z);
            this.curCharacterPos = e;
            this.outgoingEvents.Enqueue(new hEvents.CharacterPositionEventArgs(
                 e.ProcessId,
                 e.DynelType,
                 e.DynelID,
                 e.ZoneID,
                 e.ZoneName,
                 e.InShadowlands,
                 e.X,
                 e.Y,
                 e.Z,
                 e.Time));
        }

        private PersistentInfo GetPersistentInfo(int pid)
        {
            lock (this.persistentInfo)
            {
                if (!this.persistentInfo.ContainsKey(pid))
                {
                    this.persistentInfo[pid] = new PersistentInfo();
                }
                return this.persistentInfo[pid];
            }
        }

        #region Send events
        private void SendEvents(hEvents.HookEventArgs e)
        {
            switch (e.Type)
            {
                case HookEventType.CharacterPosition:
                    this.SendEvent(this.CharacterPositionEvent, e);
                    return;
                case HookEventType.DynelName:
                    this.SendEvent(this.DynelNameEvent, e);
                    return;
                case HookEventType.DebugEvent:
                    this.SendEvent(this.DebugEvent, e);
                    return;
                case HookEventType.HookStateChange:
                    this.SendEvent(this.HookStateChangeEvent, e);
                    return;
                case HookEventType.QuestLocation:
                    this.SendEvent(this.QuestLocationEvent, e);
                    break;
            }
        }

        private void SendEvent(dynamic delegates, dynamic e)
        {
            if (delegates == null) { return; }
            lock (delegates)
            {
                try
                {
                    delegates(this, e);
                }
                catch 
                {
 
                }
            }
        }
        #endregion
    }
}
