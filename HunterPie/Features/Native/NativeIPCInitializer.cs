using HunterPie.Core.Game;
using HunterPie.Core.Game.Rise;
using HunterPie.Core.Logger;
using HunterPie.Core.Native.IPC;
using HunterPie.Domain.Interfaces;
using System.Threading.Tasks;

namespace HunterPie.Features.Native
{
    internal class NativeIPCInitializer : IContextInitializer
    {
        public void Initialize(Context context)
        {
            if (context is not MHRContext)
                return;

            IPCService.Initialize().ContinueWith(async (success) =>
            {
                bool connected = await success;
                if (connected)
                    return;

                for (int i = 1; i <= 10; i++)
                {
                    Log.Debug($"Retrying to connect: Attempt {i}...");

                    connected = await IPCService.Initialize();

                    if (connected)
                        break;

                    await Task.Delay(i * 100);
                }
            });
        }
    }
}
