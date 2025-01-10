using HunterPie.Core.Crypto;
using HunterPie.Core.Domain.Cache;
using HunterPie.DI.Module;
using HunterPie.Integrations.Poogie.Account;
using HunterPie.Integrations.Poogie.Common;
using HunterPie.Integrations.Poogie.Settings;
using System.Windows;

namespace HunterPie.DI.Modules;

internal class HunterPieModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        // Intrinsic
        registry
            .WithSingle(() => Application.Current.Dispatcher)
            .WithService<PoogieConnector>()
            .WithService<InMemoryAsyncCache>()
            .WithSingle<CryptoService>();

        // Connectors
        registry
            .WithService<PoogieAccountConnector>()
            .WithService<PoogieClientSettingsConnector>();
    }
}