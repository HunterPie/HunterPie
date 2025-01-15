using HunterPie.Core.Address.Map;
using HunterPie.Core.Client;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Data;
using HunterPie.Core.Native.IPC.Handlers.Internal.Initialiaze;
using HunterPie.Core.Native.IPC.Handlers.Internal.Initialiaze.Models;
using HunterPie.Domain.Interfaces;
using HunterPie.Features.Native;
using HunterPie.Features.Patcher;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using System;
using System.Threading.Tasks;

namespace HunterPie.Game.World;

internal class MHWContextInitializer : IContextInitializer
{

    public async Task InitializeAsync(IContext context)
    {
        if (context is not MHWContext)
            return;


        InitializeGameData();
        await InitializeNativeModule(context);
    }

    private static void InitializeGameData()
    {
        MonsterData.Init(
            ClientInfo.GetPathFor("Game/World/Data/MonsterData.xml")
        );
    }

    private static async Task InitializeNativeModule(IContext context)
    {
        if (!ClientConfig.Config.Client.EnableNativeModule)
            return;

        WorldIntegrityPatcher.Patch(context);

        _ = IPCInjectorInitializer.InjectNativeModule(context);
        await NativeIPCInitializer.WaitForIPCInitialization();

        IPCInitializationMessageHandler.RequestIPCInitialization(IPCInitializationHostType.MHWorld, new[]
        {
            (UIntPtr)AddressMap.GetAbsolute("FUN_DEAL_DAMAGE"),
        });
    }
}