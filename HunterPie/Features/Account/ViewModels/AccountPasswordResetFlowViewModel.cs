using HunterPie.Core.Client.Localization;
using HunterPie.Core.Notification;
using HunterPie.Core.Notification.Model;
using HunterPie.Integrations.Poogie.Account;
using HunterPie.Integrations.Poogie.Account.Models;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.UI.Architecture;
using System;
using System.Threading.Tasks;

namespace HunterPie.Features.Account.ViewModels;

internal class AccountPasswordResetFlowViewModel(
    PoogieAccountConnector accountConnector,
    ILocalizationRepository localizationRepository) : ViewModel
{

    public bool IsRequestingCode { get; set => SetValue(ref field, value); }
    public bool HasCodeBeenSent { get; set => SetValue(ref field, value); }
    public bool IsResetInProgress { get; set => SetValue(ref field, value); }
    public bool CanChangePassword { get; set => SetValue(ref field, value); }
    public bool IsFlowActive { get; set => SetValue(ref field, value); }
    public string Email { get; set => SetValueAndUpdateState(ref field, value); } = string.Empty;
    public string Code { get; set => SetValueAndUpdateState(ref field, value); } = string.Empty;
    public string Password { get; set => SetValueAndUpdateState(ref field, value); } = string.Empty;

    public async Task RequestResetCodeAsync()
    {
        IsRequestingCode = true;

        PoogieResult<PasswordChangeResponse> response =
            await accountConnector.ForgotPasswordAsync(new PasswordResetRequest(Email: Email));

        IsRequestingCode = false;

        if (response.Error is { } error)
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
            Description: localizationRepository.FindStringBy(
                "//Strings/Client/Integrations/Poogie[@Id='PASSWORD_RESET_EMAIL_STRING']"
            ).Replace("{Email}", Email),
            DisplayTime: TimeSpan.FromSeconds(10)
        );
        await NotificationService.Show(successOptions);
        HasCodeBeenSent = true;
    }

    public async Task ChangePassword()
    {
        IsResetInProgress = true;

        PoogieResult<PasswordChangeResponse> response =
            await accountConnector.ChangePasswordAsync(new ChangePasswordRequest
            (
                Email: Email,
                Code: Code,
                NewPassword: Password
            ));

        IsResetInProgress = false;

        if (response.Error is { } error)
        {
            var errorOptions = new NotificationOptions(
                Type: NotificationType.Error,
                Title: "Error",
                Description: localizationRepository.FindByEnum(error.Code).String,
                DisplayTime: TimeSpan.FromSeconds(10)
            );
            await NotificationService.Show(errorOptions);

            return;
        }

        var successOptions = new NotificationOptions(
            Type: NotificationType.Success,
            Title: "Success",
            Description: localizationRepository.FindStringBy("//Strings/Client/Integrations/Poogie[@Id='PASSWORD_RESET_SUCCESS_STRING']"),
            DisplayTime: TimeSpan.FromSeconds(10)
        );
        await NotificationService.Show(successOptions);

        NavigateToLoginFlow();
    }

    public void NavigateToLoginFlow() => IsFlowActive = false;

    private void SetValueAndUpdateState<T>(ref T field, T value)
    {
        SetValue(ref field, value);
        CanChangePassword = Email.Length > 0 && Code.Length > 0 && Password.Length > 0;
    }
}