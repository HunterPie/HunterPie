using HunterPie.Core.Address.Map;
using HunterPie.Core.Client;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Data;
using HunterPie.Core.Native.IPC.Handlers.Internal.Initialiaze;
using HunterPie.Core.Native.IPC.Handlers.Internal.Initialiaze.Models;
using HunterPie.Domain.Interfaces;
using HunterPie.Features.Native;
using HunterPie.Features.Patcher;
using HunterPie.Integrations.Datasources.MonsterHunterRise;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Game.Rise;

internal class MHRContextInitializer : IContextInitializer
{

    private static readonly string[] Addresses =
    {
        "FUN_CALCULATE_ENTITY_DAMAGE"
    };

    public async Task InitializeAsync(IContext context)
    {
        if (context is not MHRContext)
            return;

        InitializeGameData();
        await InitializeNativeModule(context);
    }

    private static void InitializeGameData()
    {
        MonsterData.Init(
            ClientInfo.GetPathFor("Game/Rise/Data/MonsterData.xml")
        );
    }

    private static async Task InitializeNativeModule(IContext context)
    {
        if (!ClientConfig.Config.Client.EnableNativeModule)
            return;

        RiseIntegrityPatcher.Patch(context);
        RiseIntegrityPatcher.PatchProtectVirtualMemory(context);

        UIntPtr[] addresses = Addresses.Select(name => (UIntPtr)AddressMap.GetAbsolute(name))
            .ToArray();

        _ = IPCInjectorInitializer.InjectNativeModule(context);
        await NativeIPCInitializer.WaitForIPCInitialization();

        IPCInitializationMessageHandler.RequestIPCInitialization(IPCInitializationHostType.MHRise, addresses);
    }
}