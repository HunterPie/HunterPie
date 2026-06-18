using HunterPie.Core.Extensions;
using HunterPie.Features.Account.Model;
using HunterPie.Features.Account.UseCase;
using HunterPie.UI.Main.Navigators.Events;
using HunterPie.UI.Main.ViewModels;
using HunterPie.UI.Navigation;
using HunterPie.UI.SideBar.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.UI.Main.Navigators;

internal class NavigatorController(
    ISideBarCollection sideBar,
    IAppNavigationDispatcher mainNavigationDispatcher,
    IAppNavigator appNavigator,
    IBodyNavigationDispatcher bodyNavigationDispatcher,
    IAccountUseCase accountUseCase,
    MainViewModel mainViewModel,
    MainActivityViewModel mainBodyViewModel
)
{
    public async Task SetupAsync()
    {
        Subscribe();

        appNavigator.Navigate(
            viewModel: mainBodyViewModel
        );

        if (sideBar.Handlers.FirstOrDefault() is not { } sideBarElement)
            return;

        await sideBarElement.ExecuteAsync();
    }

    private void Subscribe()
    {
        mainNavigationDispatcher.NavigateRequest +=
            (_, args) => mainViewModel.ContentViewModel = args.ViewModel;
        bodyNavigationDispatcher.NavigateRequest += BodyNavigateRequest;
        accountUseCase.SessionStart += (_, e) => SetupViewModel(e.Account);
        accountUseCase.SignIn += (_, e) => SetupViewModel(e.Account);
        accountUseCase.SignOut += (_, _) => SetupViewModel(null);
    }

    private void BodyNavigateRequest(object? sender, NavigateRequestEventArgs e)
    {
        mainBodyViewModel.NavigationViewModel = e.ViewModel;

        Type viewModelType = e.ViewModel.GetType();

        sideBar.Handlers.ForEach(element => element.IsActive = viewModelType == element.ViewType);
    }

    private async void SetupViewModel(UserAccount? account)
    {
        await mainBodyViewModel.InitializeSupporterPromptAsync(account?.IsSupporter ?? false);
    }
}