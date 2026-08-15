using HunterPie.Core.Address.Map;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Game;
using HunterPie.Core.Native.IPC.Handlers.Internal.Initialize;
using HunterPie.Core.Native.IPC.Handlers.Internal.Initialize.Models;
using HunterPie.Features.Native.Service;
using HunterPie.Game.Common;
using HunterPie.Game.Rise.Patcher;
using HunterPie.Integrations.Datasources.MonsterHunterRise;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Game.Rise;

internal class MHRContextInitializer(
    IContext context,
    ClientConfig config,
    RiseIntegrityPatcher patcher,
    NativeInterfaceService nativeInterfaceService
) : IGameContextInitializer
{

    private static readonly string[] Addresses =
    {
        "FUN_CALCULATE_ENTITY_DAMAGE"
    };

    public async Task InitializeAsync()
    {
        if (context is not MHRContext)
            return;

        if (!config.EnableNativeModule)
            return;

        await patcher.Patch();
        await patcher.PatchProtectVirtualMemoryAsync();

        nint[] addresses = Addresses.Select(AddressMap.GetAbsolute)
            .ToArray();

        bool isConnected = await nativeInterfaceService.ConnectAsync();

        if (!isConnected)
            return;

        await IPCInitializationMessageHandler.RequestIPCInitializationAsync(IPCInitializationHostType.MHRise, addresses);
    }
}