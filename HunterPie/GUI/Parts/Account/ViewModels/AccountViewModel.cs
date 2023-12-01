using HunterPie.Core.Remote;
using HunterPie.Domain.Sidebar;
using HunterPie.Features.Account;
using HunterPie.Features.Account.Event;
using HunterPie.Features.Account.Model;
using HunterPie.GUI.Parts.Account.Views;
using HunterPie.GUI.Parts.Sidebar.Service;
using HunterPie.GUI.Parts.Sidebar.ViewModels;
using HunterPie.UI.Architecture;
using HunterPie.UI.Architecture.Navigator;
using System;

namespace HunterPie.GUI.Parts.Account.ViewModels;

public class AccountViewModel : ViewModel, IDisposable
{
    private bool _isAvatarClicked;
    private string _avatarUrl = "https://cdn.hunterpie.com/avatars/default.png";
    private string _username;
    private bool _isLoggedIn;
    private bool _isLoggingIn;

    public bool IsAvatarClicked { get => _isAvatarClicked; set => SetValue(ref _isAvatarClicked, value); }
    public string AvatarUrl { get => _avatarUrl; set => SetValue(ref _avatarUrl, value); }
    public string Username { get => _username; set => SetValue(ref _username, value); }
    public bool IsLoggedIn { get => _isLoggedIn; set => SetValue(ref _isLoggedIn, value); }
    public bool IsLoggingIn { get => _isLoggingIn; set => SetValue(ref _isLoggingIn, value); }

    public AccountViewModel()
    {
        AccountManager.OnSignIn += OnAccountSignIn;
        AccountManager.OnSignOut += OnAccountSignOut;
        AccountManager.OnAvatarChange += OnAvatarChange;
    }

    private void OnAvatarChange(object sender, AccountAvatarEventArgs e) => AvatarUrl = e.AvatarUrl;

    private void OnAccountSignOut(object sender, EventArgs e) => IsLoggedIn = false;

    private async void OnAccountSignIn(object sender, AccountLoginEventArgs e)
    {
        IsLoggingIn = true;
        Username = e.Account.Username;
        AvatarUrl = await CDN.GetAsset(e.Account.AvatarUrl);
        IsLoggedIn = true;
        IsLoggingIn = false;
    }

    public void NavigateToSettings()
    {
        SideBarService.Navigate(
            element: SideBar.GetInstance<SettingsSideBarElementViewModel>()
        );
    }

    public void SignOut()
    {
        IsLoggedIn = false;
        Navigator.Return();
        AccountManager.Logout();
    }

    public async void FetchAccountDetails()
    {
        IsLoggingIn = true;
        IsLoggedIn = await AccountManager.ValidateSessionToken();

        if (!IsLoggedIn)
        {
            IsLoggingIn = false;
            return;
        }

        UserAccount account = await AccountManager.FetchAccount();

        Username = account.Username;
        AvatarUrl = await CDN.GetAsset(account.AvatarUrl);

        IsLoggingIn = false;
    }

    public void OpenAccountPreferences()
    {
        var preferences = new AccountPreferencesView();
        Navigator.Navigate(preferences);
    }

    public void Dispose()
    {
        AccountManager.OnSignIn -= OnAccountSignIn;
        AccountManager.OnSignOut -= OnAccountSignOut;
        AccountManager.OnAvatarChange -= OnAvatarChange;
    }
}
