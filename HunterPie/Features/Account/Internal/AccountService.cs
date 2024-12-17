using HunterPie.Core.Domain.Cache;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Vault;
using HunterPie.Core.Vault.Model;
using HunterPie.Features.Account.Event;
using HunterPie.Features.Account.Model;
using HunterPie.Integrations.Poogie.Account;
using HunterPie.Integrations.Poogie.Account.Models;
using HunterPie.Integrations.Poogie.Common.Models;
using System;
using System.Threading.Tasks;

namespace HunterPie.Features.Account.Internal;

internal class AccountService : IAccountUseCase, IEventDispatcher
{
    private const string ACCOUNT_CACHE_KEY = "account::key";
    private readonly ICredentialVault _credentialVault;
    private readonly IAsyncCache _cache;
    private readonly PoogieAccountConnector _accountConnector;

    public event EventHandler<AccountLoginEventArgs>? SignIn;
    public event EventHandler<EventArgs>? SignOut;
    public event EventHandler<AccountLoginEventArgs>? SessionStart;
    public event EventHandler<AccountAvatarEventArgs>? AvatarChange;

    public AccountService(
        ICredentialVault credentialVault,
        IAsyncCache cache,
        PoogieAccountConnector accountConnector
    )
    {
        _credentialVault = credentialVault;
        _cache = cache;
        _accountConnector = accountConnector;
    }

    public async Task<UserAccount?> GetAsync()
    {
        Credential? credential = _credentialVault.Get();

        if (credential is not { })
            return null;

        if (await _cache.Get<UserAccount>(ACCOUNT_CACHE_KEY) is { } cachedAccount)
            return cachedAccount;

        PoogieResult<MyUserAccountResponse> result = await _accountConnector.MyUserAccount();

        if (result.Response is not { } account)
            return null;

        UserAccount model = account.ToModel();
        await _cache.Set(ACCOUNT_CACHE_KEY, model);

        this.Dispatch(
            toDispatch: SignIn,
            data: new AccountLoginEventArgs
            {
                Account = model
            }
        );

        return model;
    }

    public async Task<bool> IsValidSessionAsync()
    {
        if (await GetAsync() is not { })
            return GetSessionToken() is not null;

        _credentialVault.Delete();
        this.Dispatch(toDispatch: SignOut);
        return false;
    }

    private string? GetSessionToken()
    {
        return _credentialVault.Get()?.Password;
    }
}