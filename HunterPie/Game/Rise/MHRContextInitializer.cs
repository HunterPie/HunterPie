﻿using HunterPie.Core.Address.Map;
using HunterPie.Core.Client;
using HunterPie.Core.Game;
using HunterPie.Core.Native.IPC.Handlers.Internal.Initialize;
using HunterPie.Core.Native.IPC.Handlers.Internal.Initialize.Models;
using HunterPie.Domain.Interfaces;
using HunterPie.Features.Native;
using HunterPie.Features.Patcher;
using HunterPie.Integrations.Datasources.MonsterHunterRise;
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

        await InitializeNativeModule(context);
    }

    private static async Task InitializeNativeModule(IContext context)
    {
        if (!ClientConfig.Config.Client.EnableNativeModule)
            return;

        await RiseIntegrityPatcher.Patch(context);
        await RiseIntegrityPatcher.PatchProtectVirtualMemoryAsync(context);

        nint[] addresses = Addresses.Select(AddressMap.GetAbsolute)
            .ToArray();

        await IPCInjectorInitializer.InjectNativeModuleAsync(context);
        await NativeIPCInitializer.WaitForIPCInitialization();

        await IPCInitializationMessageHandler.RequestIPCInitializationAsync(IPCInitializationHostType.MHRise, addresses);
    }
}