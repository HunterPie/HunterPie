using HunterPie.Core.Domain.Cache;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.System.Windows.Registry;
using HunterPie.Core.System.Windows.Vault;
using HunterPie.Core.Vault;
using HunterPie.Features.Account;
using HunterPie.Features.Account.Config;
using HunterPie.Features.Account.Controller;
using HunterPie.Features.Account.Internal;
using HunterPie.GUI.Parts.Account.ViewModels;
using HunterPie.Integrations.Poogie.Account;
using HunterPie.Integrations.Poogie.Common;
using HunterPie.Integrations.Poogie.Settings;
using HunterPie.UI.Header.ViewModels;
using HunterPie.UI.Main.ViewModels;

namespace HunterPie.DI.Modules;

internal class HunterPieModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        // Intrinsic
        registry
            .WithSingle<IRemoteAccountConfigUseCase, RemoteAccountConfigService>()
            .WithSingle<RemoteConfigSyncService>()
            .WithService<IPoogieClient, PoogieConnector>()
            .WithService<IAsyncCache, InMemoryAsyncCache>()
            .WithService<ICredentialVault, WindowsCredentialVault>()
            .WithSingle<ILocalRegistry, WindowsRegistry>();

        // Connectors
        registry
            .WithService<PoogieAccountConnector>()
            .WithService<PoogieClientSettingsConnector>();

        // Account
        registry
            .WithSingle<IAccountUseCase, AccountService>()
            .WithSingle<AccountController>()
            .WithSingle<AccountMenuViewModel>()
            .WithSingle<MainApplication>();

        // ViewModels
        registry
            .WithSingle<HeaderViewModel>()
            .WithSingle<MainViewModel>()
            .WithService<AccountPasswordResetFlowViewModel>()
            .WithService<AccountLoginFlowViewModel>()
            .WithService<AccountRegisterFlowViewModel>();
    }
}