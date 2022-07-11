using HunterPie.Core.Logger;
using HunterPie.Core.Native.IPC.Handlers.Internal.Initialiaze.Models;
using HunterPie.Core.Native.IPC.Models;
using System;
using System.Runtime.InteropServices;

namespace HunterPie.Core.Native.IPC.Handlers.Internal.Initialiaze
{
    internal class IPCInitializationMessageHandler : IMessageHandler
    {
        public int Version => 1;

        public IPCMessageType Type => IPCMessageType.INIT_IPC_MEMORY_ADDRESSES;

        public void Handle(byte[] message)
        {
            IPCHookInitializationMessageHandler.RequestInitMHHooks();
        }

        public static async void RequestIPCInitialization(UIntPtr[] addresses)
        {
            UIntPtr[] buffer = new UIntPtr[256];

            Buffer.BlockCopy(addresses, 0, buffer, 0, addresses.Length * Marshal.SizeOf<UIntPtr>());

            RequestIPCInitializationMessage request = new RequestIPCInitializationMessage()
            {
                Header = new IPCMessage
                {
                    Type = IPCMessageType.INIT_IPC_MEMORY_ADDRESSES,
                    Version = 1,
                },
                Addresses = buffer,
            };

            await IPCService.Send(request);
        }

        
    }
}
