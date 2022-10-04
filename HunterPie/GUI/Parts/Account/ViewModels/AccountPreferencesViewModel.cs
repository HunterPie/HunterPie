using HunterPie.Core.Remote;
using HunterPie.Features.Account;
using HunterPie.Features.Account.Event;
using HunterPie.Features.Account.Model;
using HunterPie.UI.Architecture;
using Microsoft.Win32;
using System;
using System.Threading.Tasks;

namespace HunterPie.GUI.Parts.Account.ViewModels;

#nullable enable
public class AccountPreferencesViewModel : ViewModel, IDisposable
{
    private string _username = string.Empty;
    private string _email = string.Empty;
    private string _avatarUrl = "https://cdn.hunterpie.com/avatars/default.png";
    private bool _isSupporter;
    private bool _isFetchingAccount;
    private bool _isUploadingAvatar;

    public string Username { get => _username; set => SetValue(ref _username, value); }
    public string Email { get => _email; set => SetValue(ref _email, value); }
    public string AvatarUrl { get => _avatarUrl; set => SetValue(ref _avatarUrl, value); }
    public bool IsSupporter { get => _isSupporter; set => SetValue(ref _isSupporter, value); }
    public bool IsFetchingAccount { get => _isFetchingAccount; set => SetValue(ref _isFetchingAccount, value); }
    public bool IsUploadingAvatar { get => _isUploadingAvatar; set => SetValue(ref _isUploadingAvatar, value); }

    public AccountPreferencesViewModel()
    {
        AccountManager.OnAvatarChange += OnAvatarChange;
    }

    private async void OnAvatarChange(object? sender, AccountAvatarEventArgs e) => AvatarUrl = await CDN.GetAsset(e.AvatarUrl);

    public async void FetchAccount()
    {
        IsFetchingAccount = true;

        UserAccount? account = await AccountManager.FetchAccount();

        if (account is not null)
            await ApplyAccountInfo(account);

        IsFetchingAccount = false;
    }

    public async void UploadAvatar()
    {
        var dialog = new OpenFileDialog
        {
            InitialDirectory = Environment.CurrentDirectory,
            Filter = "Image Files|*.png;",
            RestoreDirectory = true
        };

        if (dialog.ShowDialog() != true)
            return;

        IsUploadingAvatar = true;
        await AccountManager.UploadAvatar(dialog.FileName);
        IsUploadingAvatar = false;
    }

    private async Task ApplyAccountInfo(UserAccount account)
    {
        Username = account.Username;
        Email = account.Email;
        AvatarUrl = await CDN.GetAsset(account.AvatarUrl);
        IsSupporter = account.IsSupporter;
    }

    public void Dispose() => AccountManager.OnAvatarChange -= OnAvatarChange;
}
#nullable restore
