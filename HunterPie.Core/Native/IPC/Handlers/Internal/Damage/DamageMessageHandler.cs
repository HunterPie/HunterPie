using HunterPie.Core.Native.IPC.Handlers.Internal.Damage.Models;
using HunterPie.Core.Native.IPC.Models;
using HunterPie.Core.Native.IPC.Utils;

namespace HunterPie.Core.Native.IPC.Handlers.Internal.Damage
{
    public class DamageMessageHandler : MessageDispatcher<ResponseDamageMessage>, IMessageHandler
    {
        public int Version => 1;

        public IPCMessageType Type => IPCMessageType.GET_HUNT_STATISTICS;

        public void Handle(byte[] message)
        {
            ResponseDamageMessage response = MessageHelper.Deserialize<ResponseDamageMessage>(message);
            DispatchMessage(response);
        }

        public static async void RequestHuntStatistics(long target)
        {
            RequestDamageMessage request = new RequestDamageMessage
            {
                Header = new IPCMessage
                {
                    Type = IPCMessageType.GET_HUNT_STATISTICS,
                    Version = 1,
                },
                Target = target
            };

            await IPCService.Send(request);
        }

        public static async void DeleteHuntStatisticsBy(long target)
        {
            RequestDeleteHuntStatisticsMessage request = new RequestDeleteHuntStatisticsMessage
            {
                Header = new IPCMessage
                {
                    Type = IPCMessageType.DELETE_HUNT_STATISTICS,
                    Version = 1
                },
                Target = target
            };

            await IPCService.Send(request);
        }

        public static async void ClearAllHuntStatisticsExcept(long[] targets)
        {
            RequestClearHuntStatisticsMessage request = new RequestClearHuntStatisticsMessage
            {
                Header = new IPCMessage
                {
                    Type = IPCMessageType.DELETE_HUNT_STATISTICS,
                    Version = 1
                },
                TargetsToKeep = targets
            };

            await IPCService.Send(request);
        }
    }
}
