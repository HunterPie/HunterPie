using HunterPie.Core.Logger;
using HunterPie.Core.Native.IPC;
using System.Threading.Tasks;

namespace HunterPie.Features.Native
{
    internal static class NativeIPCInitializer
    {

        public static async Task WaitForIPCInitialization()
        {
            var attempts = 0;
            while (!(await IPCService.Initialize()))
            {
                attempts++;
                Log.Debug($"Retrying to connect: Attempt {attempts}...");
                await Task.Delay(attempts * 100);
            }
        }

    }
}
