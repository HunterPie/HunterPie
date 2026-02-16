using HunterPie.Core.Client.Localization;
using HunterPie.Core.Notification;
using HunterPie.Core.Notification.Model;
using HunterPie.Integrations.Poogie.Account;
using HunterPie.Integrations.Poogie.Account.Models;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.UI.Architecture;
using System;

namespace HunterPie.Features.Account.ViewModels;

internal class AccountRegisterFlowViewModel(
    PoogieAccountConnector accountConnector,
    ILocalizationRepository localizationRepository
) : ViewModel
{
    public string Username
    {
        get;
        set
        {
            VerifyIfCanRegister();
            SetValue(ref field, value);
        }
    } = string.Empty;

    public string Email
    {
        get;
        set
        {
            VerifyIfCanRegister();
            SetValue(ref field, value);
        }
    } = string.Empty;

    public string Password
    {
        get;
        set
        {
            VerifyIfCanRegister();
            SetValue(ref field, value);
        }
    } = string.Empty;

    public bool CanRegister { get; set => SetValue(ref field, value); }

    public bool IsRegistering { get; set => SetValue(ref field, value); }

    public async void SignUp()
    {
        if (!CanRegister)
            return;

        IsRegistering = true;

        var request = new RegisterRequest(
            Username: Username,
            Email: Email,
            Password: Password
        );

        PoogieResult<RegisterResponse> register = await accountConnector.RegisterAsync(request);

        IsRegistering = false;

        if (register.Error is { } error)
        {
            var options = new NotificationOptions(
                Type: NotificationType.Error,
                Title: "Error",
                Description: localizationRepository.FindByEnum(error.Code).String,
                DisplayTime: TimeSpan.FromSeconds(10)
            );
            await NotificationService.Show(options);

            return;
        }

        var successOptions = new NotificationOptions(
            Type: NotificationType.Success,
            Title: "Success",
            Description: localizationRepository.FindStringBy("//Strings/Client/Integrations/Poogie[@Id='ACCOUNT_REGISTER_SUCCESS']")
                .Replace("{Email}", register.Response!.Email),
            DisplayTime: TimeSpan.FromSeconds(10)
        );
        await NotificationService.Show(successOptions);
    }

    private void VerifyIfCanRegister() => CanRegister = Username.Length > 0 && Email.Length > 0 && Password.Length > 0;
}