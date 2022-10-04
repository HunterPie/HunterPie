using HunterPie.Core.API;
using HunterPie.Core.API.Entities;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Vault;
using HunterPie.Core.Vault.Model;
using HunterPie.Features.Account.Event;
using HunterPie.Features.Account.Model;
using HunterPie.Features.Notification;
using HunterPie.UI.Controls.Notfication;
using System;
using System.Threading.Tasks;

namespace HunterPie.Features.Account;

#nullable enable
internal class AccountManager : IEventDispatcher
{
    private UserAccount? _cachedAccount = null;
    private static AccountManager? _instance;
    private static AccountManager Instance
    {
        get
        {
            if (_instance is null)
                _instance = new();

            return _instance;
        }
    }

    public static event EventHandler<AccountLoginEventArgs>? OnSignIn;
    public static event EventHandler<EventArgs>? OnSignOut;
    public static event EventHandler<AccountAvatarEventArgs>? OnAvatarChange;

    public static async Task<bool> ValidateSessionToken()
    {
        if (await FetchAccount() is null)
        {
            CredentialVaultService.DeleteCredential();
            Instance.Dispatch(OnSignOut);
            return false;
        }

        return GetSessionToken() is not null;
    }

    public static async Task<PoogieApiResult<LoginResponse>?> Login(LoginRequest request)
    {
        PoogieApiResult<LoginResponse>? loginResponse = await PoogieApi.Login(request);

        if (loginResponse is null || !loginResponse.Success)
            return loginResponse;

        if (loginResponse.Response is null)
            return loginResponse;

        CredentialVaultService.SaveCredential(request.Email, loginResponse.Response.Token);

        UserAccount? account = await FetchAccount();

        if (account is null)
            return null;

        AppNotificationManager.Push(
            Push.Success(
                Localization.QueryString("//Strings/Client/Integrations/Poogie[@Id='LOGIN_SUCCESS']")
                            .Replace("{Username}", account.Username)
            ),
            TimeSpan.FromSeconds(5)
        );
        Instance.Dispatch(OnSignIn, new AccountLoginEventArgs { Account = account });

        return loginResponse;
    }

    public static async void Logout()
    {
        _ = await PoogieApi.Logout();
        CredentialVaultService.DeleteCredential();
        Instance._cachedAccount = null;

        Instance.Dispatch(OnSignOut);
    }

    public static string? GetSessionToken()
    {
        Credential? credential = CredentialVaultService.GetCredential();

        return credential?.Password;
    }

    public static async Task UploadAvatar(string path)
    {
        PoogieApiResult<MyUserAccountResponse>? account = await PoogieApi.UploadAvatar(path);

        if (account is null)
            return;

        if (!account.Success)
        {
            AppNotificationManager.Push(
                Push.Error(
                    Localization.GetEnumString(account.Error!.Code)
                ),
                TimeSpan.FromSeconds(10)
            );
            return;
        }

        AppNotificationManager.Push(
            Push.Show(
                Localization.QueryString("//Strings/Client/Integrations/Poogie[@Id='AVATAR_UPLOAD_SUCCESS']")
            ),
            TimeSpan.FromSeconds(5)
        );

        Instance._cachedAccount = account.Response.ToModel();

        Instance.Dispatch(OnAvatarChange, new AccountAvatarEventArgs { AvatarUrl = Instance._cachedAccount.AvatarUrl });
    }

    public static async Task<UserAccount?> FetchAccount()
    {
        Credential? credential = CredentialVaultService.GetCredential();

        if (credential is null)
            return null;

        if (Instance._cachedAccount is not null)
            return Instance._cachedAccount;

        PoogieApiResult<MyUserAccountResponse>? account = await PoogieApi.GetMyUserAccount();

        if (account is null || account.Response is null)
            return null;

        Instance._cachedAccount = account.Response.ToModel();

        return Instance._cachedAccount;
    }
}
#nullable restore