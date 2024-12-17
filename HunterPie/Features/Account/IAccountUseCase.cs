using HunterPie.Features.Account.Event;
using HunterPie.Features.Account.Model;
using System;
using System.Threading.Tasks;

namespace HunterPie.Features.Account;

internal interface IAccountUseCase
{
    event EventHandler<AccountLoginEventArgs> SignIn;
    event EventHandler<EventArgs> SignOut;
    event EventHandler<AccountLoginEventArgs> SessionStart;
    event EventHandler<AccountAvatarEventArgs> AvatarChange;

    Task<UserAccount?> GetAsync();
    Task<bool> IsValidSessionAsync();
}