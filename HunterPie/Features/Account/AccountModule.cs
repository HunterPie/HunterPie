using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Account.Config;
using HunterPie.Features.Account.Controller;
using HunterPie.Features.Account.Service;
using HunterPie.Features.Account.ViewModels;

namespace HunterPie.Features.Account;

internal class AccountModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<AccountService>()
            .WithSingle<RemoteAccountConfigService>()
            .WithSingle<RemoteConfigSyncService>()
            .WithSingle<AccountController>()
            .WithSingle<MainApplication>()
            .WithSingle<LocalAccountConfig>()
            .WithSingle<AccountConfig>();

        registry
            .WithFactory<AccountLoginFlowViewModel>()
            .WithFactory<AccountPasswordResetFlowViewModel>()
            .WithFactory<AccountPreferencesViewModel>()
            .WithFactory<AccountRegisterFlowViewModel>()
            .WithFactory<AccountSignFlowViewModel>()
            .WithFactory<AccountVerificationResendFlowViewModel>();
    }
}