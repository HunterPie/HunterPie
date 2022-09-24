using HunterPie.Features.Account.Model;
using System;

namespace HunterPie.Features.Account.Event;
internal class AccountLoginEventArgs : EventArgs
{
    public UserAccount Account { get; init; }
}
