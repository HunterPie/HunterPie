using HunterPie.Core.Native.IPC.Handlers.Internal.Damage.Models;
using HunterPie.Core.Native.IPC.Models;
using HunterPie.Core.Native.IPC.Utils;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace HunterPie.Core.Native.IPC.Handlers.Internal.Damage;

public class DamageMessageHandler : MessageDispatcher<ResponseDamageMessage>, IMessageHandler
{
    private byte[] _lastMessageHash = { };

    public int Version => 1;

    public IPCMessageType Type => IPCMessageType.GET_HUNT_STATISTICS;

    public void Handle(byte[] message)
    {
        lock (_lastMessageHash)
        {
            if (_lastMessageHash.SequenceEqual(message))
                return;

            ResponseDamageMessage response = MessageHelper.Deserialize<ResponseDamageMessage>(message);
            DispatchMessage(response);

            _lastMessageHash = message;
        }
    }

    public static async Task RequestHuntStatisticsAsync(long target)
    {
        var request = new RequestDamageMessage
        {
            Header = new IPCMessage
            {
                Type = IPCMessageType.GET_HUNT_STATISTICS,
                Version = 1,
            },
            Target = target
        };

        _ = await IPCService.Send(request);
    }

    public static async void DeleteHuntStatisticsBy(long target)
    {
        var request = new RequestDeleteHuntStatisticsMessage
        {
            Header = new IPCMessage
            {
                Type = IPCMessageType.DELETE_HUNT_STATISTICS,
                Version = 1
            },
            Target = target
        };

        _ = await IPCService.Send(request);
    }

    public static async Task ClearAllHuntStatisticsExceptAsync(IntPtr[] targets)
    {
        nint[] buffer = new IntPtr[10];

        Buffer.BlockCopy(targets, 0, buffer, 0, targets.Length * Marshal.SizeOf<IntPtr>());

        var request = new RequestClearHuntStatisticsMessage
        {
            Header = new IPCMessage
            {
                Type = IPCMessageType.CLEAR_HUNT_STATISTICS,
                Version = 1
            },
            TargetsToKeep = buffer
        };

        _ = await IPCService.Send(request);
    }
}