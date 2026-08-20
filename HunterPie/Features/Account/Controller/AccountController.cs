using HunterPie.Core.Assets;
using HunterPie.Features.Account.Event;
using HunterPie.Features.Account.Model;
using HunterPie.Features.Account.UseCase;
using HunterPie.Features.Account.ViewModels;
using HunterPie.UI.Header.ViewModels;
using HunterPie.UI.Main.Navigators;
using System;
using System.Threading.Tasks;

namespace HunterPie.Features.Account.Controller;

internal class AccountController(
    IAccountUseCase accountUseCase,
    AccountMenuViewModel menuViewModel,
    MainBodyNavigator mainBodyNavigator,
    IAssetResolver assetResolver
)
{
    public async Task SetupAsync()
    {
        menuViewModel.IsLoading = true;
        accountUseCase.SignIn += OnSignIn;
        accountUseCase.SessionStart += OnSessionStart;
        accountUseCase.SignOut += OnSignOut;
        accountUseCase.AvatarChange += OnAvatarChange;

        UserAccount? account = await accountUseCase.GetAsync();

        if (account is { })
            UpdateViewModels(account);

        menuViewModel.IsLoading = false;
    }

    private async void OnAvatarChange(object? sender, AccountAvatarEventArgs e)
    {
        var uri = new Uri(e.AvatarUrl);

        menuViewModel.AvatarUrl = await assetResolver.Resolve(uri.AbsolutePath);
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
        var uri = new Uri(account.AvatarUrl);

        menuViewModel.Username = account.Username;
        menuViewModel.AvatarUrl = await assetResolver.Resolve(uri.AbsolutePath);
        menuViewModel.IsLoggedIn = true;
        menuViewModel.IsLoading = false;
    }
}