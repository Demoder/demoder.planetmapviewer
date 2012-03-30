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
    public static class API
    {
        /// <summary>
        /// Interfaces.dll
        /// </summary>
        public static class Interfaces
        {
            public static class N3InterfaceModule
            {
                // public: static class N3InterfaceModule_t * __cdecl N3InterfaceModule_t::GetInstanceIfAny(void)
                [DllImport("Interfaces.dll", EntryPoint = "?GetInstanceIfAny@N3InterfaceModule_t@@SAPAV1@XZ", CallingConvention = CallingConvention.Cdecl)]
                public static extern IntPtr GetInstanceIfAny();

                // public: char const * __thiscall N3InterfaceModule_t::N3Msg_GetPFName(void)const 
                [DllImport("Interfaces.dll", EntryPoint = "?N3Msg_GetPFName@N3InterfaceModule_t@@QBEPBDXZ", CallingConvention = CallingConvention.ThisCall)]
                public static extern IntPtr GetPFName(IntPtr interfacePtr);

                // public: bool __thiscall N3InterfaceModule_t::N3Msg_GetPositionData(class Identity_t const &,class Identity_t &,class Vector3_t &)const 
                /// <summary>
                /// Retrieves position information
                /// </summary>
                /// <param name="interfacePtr">Instance of n3 interface</param>
                /// <param name="characterId">What to find the position data of</param>
                /// <param name="playfieldId">Is set to current playfields identity</param>
                /// <param name="position">Is set to current characters position</param>
                /// <returns></returns>
                [DllImport("Interfaces.dll", EntryPoint = "?N3Msg_GetPositionData@N3InterfaceModule_t@@QBE_NABVIdentity_t@@AAV2@AAVVector3_t@@@Z", CallingConvention = CallingConvention.ThisCall)]
                public static extern bool GetPositionData(
                 IntPtr interfacePtr,
                 [In, MarshalAs(UnmanagedType.LPStruct)]Identity characterId,
                 [Out, MarshalAs(UnmanagedType.LPStruct)]Identity playfieldId,
                 [Out, MarshalAs(UnmanagedType.LPStruct)]Vector3 position);


                // public: char const * __thiscall N3InterfaceModule_t::N3Msg_GetName(class Identity_t const &,class Identity_t const &)const
                [DllImport("Interfaces.dll", EntryPoint = "?N3Msg_GetName@N3InterfaceModule_t@@QBEPBDABVIdentity_t@@0@Z", CallingConvention = CallingConvention.ThisCall)]
                public static extern IntPtr GetName(
                  IntPtr interfacePtr,
                  [In, MarshalAs(UnmanagedType.LPStruct)]  Identity id1,
                  [In, MarshalAs(UnmanagedType.LPStruct)]  Identity id2);


                // public: int __thiscall N3InterfaceModule_t::N3Msg_GetSkill(enum GameData::Stat_e,int)const 
                [DllImport("Interfaces.dll", EntryPoint = "?N3Msg_GetSkill@N3InterfaceModule_t@@QBEHW4Stat_e@GameData@@H@Z", CallingConvention = CallingConvention.ThisCall)]
                public static extern int GetSkill(
                    IntPtr interfacePtr,
                    int skill,
                    int unknown);
            }

            public static class Client_t
            {
                // public: void __thiscall Client_t::FrameProcess(void)
                // thiscall means first argument is pointer to instance
                [DllImport("Interfaces.dll", EntryPoint = "?FrameProcess@Client_t@@QAEXXZ", CallingConvention = CallingConvention.ThisCall)]
                public static extern void FrameProcess(IntPtr clientPtr);

                //public: static unsigned int __cdecl Client_t::GetCharID(void)
                [DllImport("Interfaces.dll", EntryPoint = "?GetCharID@Client_t@@SAIXZ", CallingConvention=CallingConvention.Cdecl)]
                public static extern uint GetCharID();
                

                #region Delegates
                [UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Unicode, SetLastError = true)]
                public delegate void AoFrameProcessDelegate(IntPtr clientPtr);
                #endregion

            }
        }
    }
}
