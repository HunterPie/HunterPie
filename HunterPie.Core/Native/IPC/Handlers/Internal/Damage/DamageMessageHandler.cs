using HunterPie.Core.Native.IPC.Handlers.Internal.Damage.Models;
using HunterPie.Core.Native.IPC.Models;
using HunterPie.Core.Native.IPC.Utils;

namespace HunterPie.Core.Native.IPC.Handlers.Internal.Damage
{
    public class DamageMessageHandler : MessageDispatcher<ResponseDamageMessage>, IMessageHandler
    {
        public int Version => 1;

        public IPCMessageType Type => IPCMessageType.DAMAGE;

        public void Handle(byte[] message)
        {
            ResponseDamageMessage response = MessageHelper.Deserialize<ResponseDamageMessage>(message);
            DispatchMessage(response);
        }

        public static async void Request(long target)
        {
            RequestDamageMessage request = new RequestDamageMessage
            {
                Header = new IPCMessage
                {
                    Type = IPCMessageType.DAMAGE,
                    Version = 1,
                },
                Target = target
            };

            await IPCService.Send(request);
        }
    }
}
