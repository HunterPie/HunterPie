using HunterPie.Core.Remote;
using HunterPie.Features.Account.Event;
using HunterPie.Features.Account.Model;
using HunterPie.UI.Header.ViewModels;
using System;

namespace HunterPie.Features.Account.Controller;

internal class AccountController
{
    private readonly AccountMenuViewModel _menuViewModel = new() { IsLoading = true };

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

    public AccountMenuViewModel GetAccountMenuViewModel()
    {
        return _menuViewModel;
    }
}