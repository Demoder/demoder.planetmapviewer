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
                    EasyHookEntryPoint myInstance = null;
                    
                    try
                    {
                        myInstance = HookRuntimeInfo.Callback as EasyHookEntryPoint;
                        if (myInstance == null)
                        {
                            return;
                        }

                        bool checkPosition = myInstance.DataStore.TimeSinceLastPositionQuery.ElapsedMilliseconds >= 100;

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

                        if (n3interface.ToInt32() == 0)
                        {
                            return;
                        }

                        var charID = AONative.API.Interfaces.Client_t.GetCharID();
                        if (charID == 0)
                        {
                            // There's no logged in character
                            if (myInstance.DataStore.CurrentCharacter == null)
                            {
                                // and we haven't reported any characters yet.
                                return;
                            }
                            if (myInstance.DataStore.CurrentCharacter.DynelID == charID) 
                            {
                                // and the previously reported character was 0
                                return;
                            }

                            // No longer tracking a character.
                            // Make sure we're notifying AoHook about 'character logout'
                            var e = new CharacterLogoutEventArgs(myInstance.DataStore.CurrentCharacter.ServerID, myInstance.DataStore.CurrentCharacter.DynelID);
                            myInstance.SendBridgeEvent(e);

                            // Reset reported character
                            myInstance.DataStore.CurrentCharacter = null;
                            myInstance.DataStore.ServerID = 0;
                            myInstance.DataStore.CharacterID = 0;
                        }


                        // Get server ID
                        var serverId = AONative.API.Interfaces.Client_t.GetServerID(clientPtr);

                        // Need to report login?
                        if (charID != 0 && serverId != 1)
                        {
                            if (myInstance.DataStore.CharacterID == 0)
                            {
                                // Report login.
                                var e2 = new CharacterLoginEventArgs(serverId, charID);
                                myInstance.DataStore.CharacterID = charID;
                                myInstance.SendBridgeEvent(e2);
                            }
                            else if (myInstance.DataStore.CharacterID != charID)
                            {
                                // Send logout for previous character
                                var e1 = new CharacterLogoutEventArgs(myInstance.DataStore.ServerID, myInstance.DataStore.CharacterID);
                                myInstance.SendBridgeEvent(e1);

                                // Send login for new character.
                                var e2 = new CharacterLoginEventArgs(serverId, charID);
                                myInstance.DataStore.CharacterID = charID;
                                myInstance.SendBridgeEvent(e2);
                            }
                        }
                        
                        if (myInstance.DataStore.ServerID != serverId)
                        {
                            myInstance.DataStore.ServerID = serverId;
                            myInstance.SendBridgeEvent(new ServerIdEventArgs(serverId));
                        }

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

                            // Are we in shadowlands?
                            bool inShadowlands = AONative.API.Interfaces.N3InterfaceModule.GetSkill(n3interface, (int)CharacterSkill.ExpansionPlayfield, 0) == 1;

                            // Send it across the bridge
                            myInstance.SendBridgeEvent(new DynelPositionEventArgs(
                                myInstance.DataStore.ServerID,
                                Identity.Character,
                                charID,
                                pfIdentity.Instance,
                                Marshal.PtrToStringAnsi(AONative.API.Interfaces.N3InterfaceModule.GetPFName(n3interface)),
                                inShadowlands,
                                position.X,
                                position.Y,
                                position.Z));
                            myInstance.DataStore.TimeSinceLastPositionQuery.Reset();
                            myInstance.DataStore.TimeSinceLastPositionQuery.Start();
                        }
                        #endregion

                        if (myInstance.DataStore.CurrentCharacter == null
                            || myInstance.DataStore.CurrentCharacter.DynelID != charID
                            || myInstance.DataStore.ServerID != serverId)
                        {

                            // Tracking character still...
                            var charName = BridgeHelperMethods.GetName(Identity.Character, charID);
                            if (!String.IsNullOrEmpty(charName) && charName != "NoName")
                            {
                                myInstance.DataStore.CurrentCharacter = new DynelNameEventArgs(serverId, Identity.Character, charID, charName, true);
                                myInstance.SendBridgeEvent(myInstance.DataStore.CurrentCharacter);
                            }
                        }

                        IntPtr pfNamePointer = API.Interfaces.N3InterfaceModule.GetPFName(n3interface);
                        string pfName = Marshal.PtrToStringAnsi(pfNamePointer);
                        //myInstance.Debug(true, "pfName: {0}\r\n", pfName);

                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            if (myInstance != null)
                            {
                                myInstance.Debug(true, "AoFrameProcess: {0} \r\n {1}", ex.ToString(), ex.StackTrace ?? "");
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

            public static class N3InterfaceModule
            {
                public static bool GetQuestWorldPos(IntPtr interfacePtr, Identity identityMission, Identity identityZone, Vector3 worldCoord, Vector3 zoneCoord)
                {
                    bool ret = false;
                    var myInstance = HookRuntimeInfo.Callback as EasyHookEntryPoint;
                    try
                    {
                        ret = AONative.API.Interfaces.N3InterfaceModule.GetQuestWorldPos(interfacePtr, identityMission, identityZone, worldCoord, zoneCoord);
                        if (myInstance == null) { return ret; }

                        // Insert code to send bridge event here
                        myInstance.SendBridgeEvent(
                            new QuestLocationEventArgs(
                                identityMission.Instance,
                                identityZone.Instance,
                                worldCoord,
                                zoneCoord));
                    }
                    catch (Exception ex)
                    {
                        if (myInstance != null)
                        {
                            myInstance.Debug(true, "GetWorldPos Exception: {0}", ex.ToString());
                        }
                    }
                    return ret;
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
