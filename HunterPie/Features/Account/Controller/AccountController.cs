using HunterPie.Core.Remote;
using HunterPie.Features.Account.Event;
using HunterPie.Features.Account.Model;
using HunterPie.GUI.Parts.Account.ViewModels;
using HunterPie.UI.Header.ViewModels;
using HunterPie.UI.Logging.ViewModels;
using HunterPie.UI.Navigation;
using System;
using System.Threading.Tasks;

namespace HunterPie.Features.Account.Controller;

internal class AccountController
{
    private readonly AccountMenuViewModel _menuViewModel = new() { IsLoading = true };
    private static AccountPreferencesViewModel? _preferencesViewModel;

    public AccountController()
    {
        AccountManager.OnSignIn += OnSignIn;
        AccountManager.OnSessionStart += OnSessionStart;
        AccountManager.OnSignOut += OnSignOut;
        AccountManager.OnAvatarChange += OnAvatarChange;
    }

    private async void OnAvatarChange(object? sender, AccountAvatarEventArgs e)
    {
        _menuViewModel.AvatarUrl = await CDN.GetAsset(e.AvatarUrl);
    }

    private void OnSignOut(object? sender, EventArgs e)
    {
        _menuViewModel.IsLoggedIn = false;
        _menuViewModel.IsLoading = false;

        //Navigator.Body.Return();
    }

    private void OnSessionStart(object? sender, AccountLoginEventArgs e) => UpdateViewModels(e.Account);

    private void OnSignIn(object? sender, AccountLoginEventArgs e) => UpdateViewModels(e.Account);

    private async void UpdateViewModels(UserAccount account)
    {
        _menuViewModel.Username = account.Username;
        _menuViewModel.AvatarUrl = await CDN.GetAsset(account.AvatarUrl);
        _menuViewModel.IsLoggedIn = true;
        _menuViewModel.IsLoading = false;
    }

    public static async Task<AccountPreferencesViewModel> GetPreferencesViewModel()
    {
        UserAccount? account = await AccountManager.FetchAccount();

        _preferencesViewModel = new AccountPreferencesViewModel { IsFetchingAccount = true };

        if (account is null)
            return _preferencesViewModel;

        return new AccountPreferencesViewModel
        {
            Email = account.Email,
            Username = account.Username,
            AvatarUrl = await CDN.GetAsset(account.AvatarUrl),
            IsSupporter = account.IsSupporter,
            IsFetchingAccount = false
        };
    }

    public AccountMenuViewModel GetAccountMenuViewModel()
    {
        return _menuViewModel;
    }
}