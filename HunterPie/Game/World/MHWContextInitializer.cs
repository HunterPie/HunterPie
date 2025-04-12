using HunterPie.Core.Address.Map;
using HunterPie.Core.Client;
using HunterPie.Core.Game;
using HunterPie.Core.Native.IPC.Handlers.Internal.Initialize;
using HunterPie.Core.Native.IPC.Handlers.Internal.Initialize.Models;
using HunterPie.Domain.Interfaces;
using HunterPie.Features.Native;
using HunterPie.Features.Patcher;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using System.Threading.Tasks;

namespace HunterPie.Game.World;

internal class MHWContextInitializer : IContextInitializer
{

    public async Task InitializeAsync(IContext context)
    {
        if (context is not MHWContext)
            return;

        await InitializeNativeModuleAsync(context);
    }

    private static async Task InitializeNativeModuleAsync(IContext context)
    {
        if (!ClientConfig.Config.Client.EnableNativeModule)
            return;

        WorldIntegrityPatcher.Patch(context);

        await IPCInjectorInitializer.InjectNativeModuleAsync(context);
        await NativeIPCInitializer.WaitForIPCInitialization();

        await IPCInitializationMessageHandler.RequestIPCInitializationAsync(
            hostType: IPCInitializationHostType.MHWorld,
            addresses: new[] { AddressMap.GetAbsolute("FUN_DEAL_DAMAGE") }
        );
    }
}