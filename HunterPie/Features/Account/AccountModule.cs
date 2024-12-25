using HunterPie.DI;
using HunterPie.DI.Modules;
using HunterPie.Features.Account.Config;
using HunterPie.Features.Account.Controller;
using HunterPie.Features.Account.Service;
using HunterPie.Features.Account.UseCase;
using HunterPie.GUI.Parts.Account.ViewModels;
using HunterPie.UI.Header.ViewModels;

namespace HunterPie.Features.Account;

internal class AccountModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<IAccountUseCase, AccountService>()
            .WithSingle<IRemoteAccountConfigUseCase, RemoteAccountConfigService>()
            .WithSingle<RemoteConfigSyncService>()
            .WithSingle<AccountController>()
            .WithSingle<MainApplication>();

        registry
            .WithSingle<AccountMenuViewModel>()
            .WithService<AccountPasswordResetFlowViewModel>()
            .WithService<AccountLoginFlowViewModel>()
            .WithService<AccountRegisterFlowViewModel>();
    }
}