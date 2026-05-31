using HunterPie.Core.Address.Map;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Game;
using HunterPie.Core.Native.IPC.Handlers.Internal.Initialize;
using HunterPie.Core.Native.IPC.Handlers.Internal.Initialize.Models;
using HunterPie.Features.Native;
using HunterPie.Game.Common;
using HunterPie.Game.World.Patcher;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using System.Threading.Tasks;

namespace HunterPie.Game.World;

internal class MHWContextInitializer(
    IContext context,
    ClientConfig config,
    WorldIntegrityPatcher patcher
) : IGameContextInitializer
{

    public async Task InitializeAsync()
    {
        if (context is not MHWContext)
            return;

        if (!config.EnableNativeModule)
            return;

        patcher.Patch();

        await IPCInjectorInitializer.InjectNativeModuleAsync(context);
        await NativeIPCInitializer.WaitForIPCInitialization();

        await IPCInitializationMessageHandler.RequestIPCInitializationAsync(
            hostType: IPCInitializationHostType.MHWorld,
            addresses: [AddressMap.GetAbsolute("FUN_DEAL_DAMAGE")]
        );
    }
}