using HunterPie.Core.Address.Map;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Rise;
using HunterPie.Core.Native.IPC.Handlers.Internal.Initialiaze;
using HunterPie.Domain.Interfaces;
using HunterPie.Features.Native;
using HunterPie.Features.Patcher;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Game.Rise;

internal class MHRContextInitializer : IContextInitializer
{

    private static readonly string[] _addresses =
    {
        "FUN_CALCULATE_ENTITY_DAMAGE"
    };

    /// <inheritdoc />
    public async Task InitializeAsync(Context context)
    {
        if (context is not MHRContext)
            return;

        RiseIntegrityPatcher.Patch(context);
        // Make sure to inject module after patching.
        _ = IPCInjectorInitializer.InjectNativeModule(context);
        await NativeIPCInitializer.WaitForIPCInitialization();
        UIntPtr[] addresses = _addresses.Select(name => (UIntPtr)AddressMap.GetAbsolute(name))
            .ToArray();
        IPCInitializationMessageHandler.RequestIPCInitialization(addresses);
    }
}