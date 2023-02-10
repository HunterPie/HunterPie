using HunterPie.Core.Client.Localization;
using HunterPie.Features.Account;
using HunterPie.Features.Notification;
using HunterPie.Integrations.Poogie.Account.Models;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.UI.Architecture;
using HunterPie.UI.Controls.Notfication;
using System;
using System.Threading.Tasks;

namespace HunterPie.GUI.Parts.Account.ViewModels;

#nullable enable
public class AccountLoginFlowViewModel : ViewModel
{
    private string _email = "";
    private string _password = "";
    private bool _canLogIn;
    private bool _isLoggingIn;

    public string Email
    {
        get => _email;
        set
        {
            CanLogIn = Password.Length > 0 && value.Length > 0;
            SetValue(ref _email, value);
        }
    }
    public string Password
    {
        get => _password;
        set
        {
            CanLogIn = Email.Length > 0 && value.Length > 0;
            SetValue(ref _password, value);
        }
    }
    public bool IsLoggingIn { get => _isLoggingIn; set => SetValue(ref _isLoggingIn, value); }
    public bool CanLogIn { get => _canLogIn; set => SetValue(ref _canLogIn, value); }
    public AccountPasswordResetFlowViewModel PasswordResetFlowViewModel { get; } = new();

    public async Task<bool> SignIn()
    {
        if (!CanLogIn)
            return false;

        IsLoggingIn = true;
        CanLogIn = false;

        var request = new LoginRequest
        (
            Email: Email,
            Password: Password
        );

        PoogieResult<LoginResponse>? result = await AccountManager.Login(request);

        IsLoggingIn = false;
        CanLogIn = true;

        if (result is null)
            return false;

        if (result.Error is { } error)
        {
            AppNotificationManager.Push(
                Push.Error(
                    Localization.GetEnumString(error.Code)
                ),
                TimeSpan.FromSeconds(5)
            );

            return false;
        }

        return true;
    }

    public void NavigateToPasswordResetFlow() => PasswordResetFlowViewModel.IsFlowActive = true;
}
