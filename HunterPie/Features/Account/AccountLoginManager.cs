using HunterPie.Core.API;
using HunterPie.Core.API.Entities;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Vault;
using HunterPie.Core.Vault.Model;
using HunterPie.Features.Account.Event;
using HunterPie.Features.Account.Model;
using System;
using System.Threading.Tasks;

namespace HunterPie.Features.Account;

#nullable enable
internal class AccountLoginManager : IEventDispatcher
{
    private static AccountLoginManager? _instance;
    private static AccountLoginManager Instance
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

    public static async Task<UserAccount?> Login(LoginRequest request)
    {
        LoginResponse? loginResponse = await PoogieApi.Login(request);

        if (loginResponse is null)
            return null;

        CredentialVaultService.SaveCredential(request.Username, loginResponse.Token);

        UserAccount? account = await FetchAccount();

        if (account is null)
            return null;

        Instance.Dispatch(OnSignIn, new AccountLoginEventArgs { Account = account });

        return account;
    }

    public static async void Logout()
    {
        _ = await PoogieApi.Logout();
        CredentialVaultService.DeleteCredential();

        Instance.Dispatch(OnSignOut);
    }

    public static string? GetSessionToken()
    {
        Credential? credential = CredentialVaultService.GetCredential();

        return credential?.Password;
    }

    public static async Task<UserAccount?> FetchAccount()
    {
        Credential? credential = CredentialVaultService.GetCredential();

        return credential is null ? null : ((await PoogieApi.GetMyUserAccount())?.ToModel());
    }
}
#nullable restore