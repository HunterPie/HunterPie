using System;
using System.Threading.Tasks;
using HunterPie.Core.Address.Map;
using HunterPie.Core.Client;
using HunterPie.Core.Game;
using HunterPie.Core.Game.World;
using HunterPie.Core.Native.IPC.Handlers.Internal.Initialiaze;
using HunterPie.Core.Native.IPC.Handlers.Internal.Initialiaze.Models;
using HunterPie.Domain.Interfaces;
using HunterPie.Features.Native;
using HunterPie.Features.Patcher;

namespace HunterPie.Game.World;

internal class MHWContextInitializer : IContextInitializer
{

    /// <inheritdoc />
    public async Task InitializeAsync(Context context)
    {
        if (context is not MHWContext) return;

        if (!ClientConfig.Config.Client.EnableNativeModule) return;

        WorldIntegrityPatcher.Patch(context);
        // Make sure to inject module after patching.
        IPCInjectorInitializer.InjectNativeModule(context);
        await NativeIPCInitializer.WaitForIPCInitialization();
        IPCInitializationMessageHandler.RequestIPCInitialization(IPCInitializationHostType.MHWorld, new[]
        {
            (UIntPtr)AddressMap.GetAbsolute("FUN_DEAL_DAMAGE"),
        });
    }

}
