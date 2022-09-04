using HunterPie.Core.Native.IPC.Models;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace HunterPie.Core.Native.IPC.Handlers.Internal.Initialiaze.Models
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RequestIPCInitializationMessage
    {
        public IPCMessage Header;

        public IPCInitializationHostType HostType;

        private readonly int Reserved1;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public UIntPtr[] Addresses;
    }

    public enum IPCInitializationHostType
    {
        Invalid = 0,
        /// <summary>Monster Hunter World, Monster Hunter World: Iceborne.</summary>
        MHWorld,
        /// <summary>Monster Hunter Rise, Monster Hunter World: Sunbreak.</summary>
        MHRise,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ResponseIPCInitializationMessage
    {
        public IPCMessage Header;

        public int HResult;
    }
}
