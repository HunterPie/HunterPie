using HunterPie.Core.Domain.Cache;
using HunterPie.Features.Account;
using HunterPie.Features.Account.Internal;
using HunterPie.Integrations.Poogie.Account;
using HunterPie.Integrations.Poogie.Common;
using HunterPie.UI.Header.ViewModels;
using HunterPie.UI.Main.ViewModels;

namespace HunterPie.DI.Modules;

internal class HunterPieModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithService<IPoogieClient, PoogieConnector>()
            .WithService<IAsyncCache, InMemoryAsyncCache>()
            .WithService<PoogieAccountConnector>()
            .WithSingle<IAccountUseCase, AccountService>()
            .WithSingle<AccountMenuViewModel>()
            .WithSingle<MainApplication>();

        registry
            .WithSingle<HeaderViewModel>()
            .WithSingle<MainViewModel>();
    }
}
