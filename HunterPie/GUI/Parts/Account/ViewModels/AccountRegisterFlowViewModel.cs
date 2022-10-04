using HunterPie.Core.API;
using HunterPie.Core.API.Entities;
using HunterPie.Core.Client.Localization;
using HunterPie.Features.Notification;
using HunterPie.UI.Architecture;
using HunterPie.UI.Controls.Notfication;
using System;

namespace HunterPie.GUI.Parts.Account.ViewModels;
public class AccountRegisterFlowViewModel : ViewModel
{
    private string _username = string.Empty;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private bool _canRegister;
    private bool _isRegistering;

    public string Username
    {
        get => _username;
        set
        {
            VerifyIfCanRegister();
            SetValue(ref _username, value);
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            VerifyIfCanRegister();
            SetValue(ref _email, value);
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            VerifyIfCanRegister();
            SetValue(ref _password, value);
        }
    }

    public bool CanRegister { get => _canRegister; set => SetValue(ref _canRegister, value); }

    public bool IsRegistering { get => _isRegistering; set => SetValue(ref _isRegistering, value); }

    public async void SignUp()
    {
        if (!CanRegister)
            return;

        IsRegistering = true;

        var request = new RegisterRequest
        {
            Username = Username,
            Email = Email,
            Password = Password
        };

        PoogieApiResult<RegisterResponse> register = await PoogieApi.Register(request);

        IsRegistering = false;

        if (!register.Success)
        {
            AppNotificationManager.Push(
                Push.Error(
                    Localization.GetEnumString(register.Error!.Code)
                ),
                TimeSpan.FromSeconds(5)
            );

            return;
        }

        AppNotificationManager.Push(
            Push.Success(
                Localization.QueryString("//Strings/Client/Integrations/Poogie[@Id='ACCOUNT_REGISTER_SUCCESS']")
                            .Replace("{Email}", register.Response.Email)
            ),
            TimeSpan.FromSeconds(10)
        );
    }

    private void VerifyIfCanRegister() => CanRegister = Username.Length > 0 && Email.Length > 0 && Password.Length > 0;
}
