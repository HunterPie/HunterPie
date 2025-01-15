using HunterPie.Core.Logger;
using HunterPie.Core.Native.IPC.Handlers.Internal.Initialiaze.Models;
using HunterPie.Core.Native.IPC.Models;
using HunterPie.Core.Native.IPC.Utils;
using System;
using System.Runtime.InteropServices;

namespace HunterPie.Core.Native.IPC.Handlers.Internal.Initialiaze;

internal class IPCInitializationMessageHandler : IMessageHandler
{
    private const string ERROR_DIALOG_MESSAGE = "HunterPie has detected wrong version of HunterPie Native Interface currently in the game.\nYou must restart your game for it to work properly";
    public int Version => 2;

    public IPCMessageType Type => IPCMessageType.INIT_IPC_MEMORY_ADDRESSES;

    public void Handle(byte[] message)
    {
        ResponseIPCInitializationMessage response = MessageHelper.Deserialize<ResponseIPCInitializationMessage>(message);

        if (response.Header.Version != Version)
        {
            Log.Warn(ERROR_DIALOG_MESSAGE);
            return;
        }

        // Not throwing Exception here since stack trace won't be helpful.
        Exception ex = Marshal.GetExceptionForHR(response.HResult);
        if (ex != null)
        {
            Log.Error("Failed to initialize IPC: {0}", ex);
            Log.Warn(ERROR_DIALOG_MESSAGE);
            return;
        }

        IPCHookInitializationMessageHandler.RequestInitMHHooks();
    }

    public static async void RequestIPCInitialization(IPCInitializationHostType hostType, UIntPtr[] addresses)
    {
        nuint[] buffer = new UIntPtr[256];

        Buffer.BlockCopy(addresses, 0, buffer, 0, addresses.Length * Marshal.SizeOf<UIntPtr>());

        var request = new RequestIPCInitializationMessage()
        {
            Header = new IPCMessage
            {
                Type = IPCMessageType.INIT_IPC_MEMORY_ADDRESSES,
                Version = 1,
            },
            HostType = hostType,
            Addresses = buffer,
        };

        _ = await IPCService.Send(request);
    }
}