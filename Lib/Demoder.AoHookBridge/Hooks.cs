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
using Demoder.AoHookBridge.AONative;
using System.Runtime.InteropServices;
using Demoder.AoHookBridge.Events;
using System.Threading;
using System.Diagnostics;

namespace Demoder.AoHookBridge
{
    public static class Hooks
    {
        /// <summary>
        /// Interfaces.dll
        /// </summary>
        public static class Interfaces
        {
            public static class Client_t
            {               
                public static void AoFrameProcess(IntPtr clientPtr)
                {
                    var myInstance = HookRuntimeInfo.Callback as EasyHookEntryPoint;
                    if (myInstance == null) { return; }
                    try
                    {
                        //bool checkPosition = timeSinceLastPositionQuery.ElapsedMilliseconds >= 67;
                        bool checkPosition = myInstance.DataStore.TimeSinceLastPositionQuery.ElapsedMilliseconds >= 67;

                        // If there's nothing to do, return now.
                        if (!checkPosition &&
                            myInstance.DataStore.CurrentCharacter != null)
                        {
                            return;
                        }

                        bool doDebugMsgs = false;
                        myInstance.Debug(doDebugMsgs, "AoFrameProcessHook() begin");
                        IntPtr n3interface = AONative.API.Interfaces.N3InterfaceModule.GetInstanceIfAny();
                        myInstance.Debug(doDebugMsgs, "n3interface: {0}", n3interface.ToString());

                        if (n3interface.ToInt32() != 0)
                        {
                            var charID = AONative.API.Interfaces.Client_t.GetCharID();
                            if (charID != 0)
                            {

                                #region Check Position
                                // Should we check position?
                                if (checkPosition)
                                {
                                    // Playfield type/id will be stored here
                                    var pfIdentity = new Identity();
                                    // Dynel position will be stored here
                                    var position = new Vector3();

                                    // Retrieve position data
                                    AONative.API.Interfaces.N3InterfaceModule.GetPositionData(
                                        n3interface,
                                        new Identity(Identity.Character, charID),
                                        pfIdentity,
                                        position);
                                    // Send it across the bridge
                                    myInstance.SendBridgeEvent(new DynelPositionEventArgs(
                                        Identity.Character,
                                        charID,
                                        pfIdentity.ID,
                                        Marshal.PtrToStringAnsi(AONative.API.Interfaces.N3InterfaceModule.GetPFName(n3interface)),
                                        position.X,
                                        position.Y,
                                        position.Z));
                                    myInstance.DataStore.TimeSinceLastPositionQuery.Reset();
                                    myInstance.DataStore.TimeSinceLastPositionQuery.Start();
                                }
                                #endregion

                                if (myInstance.DataStore.CurrentCharacter == null)
                                {
                                    var charName = BridgeHelperMethods.GetName(Identity.Character, charID);
                                    if (!String.IsNullOrEmpty(charName) && charName != "NoName")
                                    {
                                        myInstance.DataStore.CurrentCharacter = new DynelNameEventArgs(Identity.Character, charID, charName, true);
                                        myInstance.SendBridgeEvent(myInstance.DataStore.CurrentCharacter);
                                    }
                                }
                            }

                            IntPtr pfNamePointer = API.Interfaces.N3InterfaceModule.GetPFName(n3interface);
                            string pfName = Marshal.PtrToStringAnsi(pfNamePointer);
                            //myInstance.Debug(true, "pfName: {0}\r\n", pfName);
                        }
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            if (myInstance != null)
                            {
                                myInstance.Debug(true, "AoFrameProcess: {0}", ex.ToString());
                            }
                        }
                        catch { }
                    }
                    finally
                    {
                        AONative.API.Interfaces.Client_t.FrameProcess(clientPtr);
                    }
                }
            }
        }

        public static class BridgeHelperMethods
        {
            public static string GetName(uint type, uint id)
            {
                string retVal = "NoName";
                // Try to get target name
                var n3instance = API.Interfaces.N3InterfaceModule.GetInstanceIfAny();
                if (n3instance.ToInt32() != 0)
                {
                    var blankId = new Identity();
                    var strPtr = API.Interfaces.N3InterfaceModule.GetName(
                        n3instance,
                        new Identity(type, id),
                        blankId
                        );
                    if (strPtr.ToInt32() != 0)
                    {
                        retVal = Marshal.PtrToStringAnsi(strPtr);
                    }
                }
                return retVal;
            }
        }
    }
}
