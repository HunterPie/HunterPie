using System.Runtime.InteropServices;
using HunterPie.Core.Native.IPC.Models;

namespace HunterPie.Core.Native.IPC.Handlers.Internal.Initialiaze.Models;

[StructLayout(LayoutKind.Sequential)]
public struct ResponseIPCInitializationMessage
{
    public IPCMessage Header;

    public int HResult;
}
