using HunterPie.Core.Domain.Cache;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.System.Windows.Registry;
using HunterPie.Core.System.Windows.Vault;
using HunterPie.Core.Vault;
using HunterPie.Integrations.Poogie.Account;
using HunterPie.Integrations.Poogie.Common;
using HunterPie.Integrations.Poogie.Settings;

namespace HunterPie.DI.Modules;

internal class HunterPieModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        // Intrinsic
        registry
            .WithService<IPoogieClient, PoogieConnector>()
            .WithService<IAsyncCache, InMemoryAsyncCache>()
            .WithService<ICredentialVault, WindowsCredentialVault>()
            .WithSingle<ILocalRegistry, WindowsRegistry>();

        // Connectors
        registry
            .WithService<PoogieAccountConnector>()
            .WithService<PoogieClientSettingsConnector>();
    }
}