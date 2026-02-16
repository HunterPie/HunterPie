using HunterPie.Core.Remote;
using HunterPie.Features.Account.Event;
using HunterPie.Features.Account.Model;
using HunterPie.Features.Account.UseCase;
using HunterPie.Features.Account.ViewModels;
using HunterPie.UI.Architecture.Extensions;
using HunterPie.UI.Header.ViewModels;
using HunterPie.UI.Main.Navigators;
using System;
using System.Threading.Tasks;

namespace HunterPie.Features.Account.Controller;

internal class AccountController(
    IAccountUseCase accountUseCase,
    AccountMenuViewModel menuViewModel,
    MainBodyNavigator mainBodyNavigator
)
{
    public async Task SetupAsync()
    {
        menuViewModel.Apply(it => it.IsLoading = true);
        accountUseCase.SignIn += OnSignIn;
        accountUseCase.SessionStart += OnSessionStart;
        accountUseCase.SignOut += OnSignOut;
        accountUseCase.AvatarChange += OnAvatarChange;

        UserAccount? account = await accountUseCase.GetAsync();

        if (account is { })
            UpdateViewModels(account);
    }

    private async void OnAvatarChange(object? sender, AccountAvatarEventArgs e)
    {
        menuViewModel.AvatarUrl = await CDN.GetAsset(e.AvatarUrl);
    }

    private void OnSignOut(object? sender, EventArgs e)
    {
        menuViewModel.IsLoggedIn = false;
        menuViewModel.IsLoading = false;

        mainBodyNavigator.ReturnWhen<AccountPreferencesViewModel>();
    }

    private void OnSessionStart(object? sender, AccountLoginEventArgs e) => UpdateViewModels(e.Account);

    private void OnSignIn(object? sender, AccountLoginEventArgs e) => UpdateViewModels(e.Account);

    private async void UpdateViewModels(UserAccount account)
    {
        menuViewModel.Username = account.Username;
        menuViewModel.AvatarUrl = await CDN.GetAsset(account.AvatarUrl);
        menuViewModel.IsLoggedIn = true;
        menuViewModel.IsLoading = false;
    }
}