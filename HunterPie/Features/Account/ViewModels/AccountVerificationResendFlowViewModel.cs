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

internal class AccountVerificationResendFlowViewModel(
    PoogieAccountConnector accountConnector,
    ILocalizationRepository localizationRepository) : ViewModel
{
    public bool IsRequestingVerification
    {
        get;
        set => SetValue(ref field, value);
    }

    public bool CanRequestVerification
    {
        get;
        set => SetValue(ref field, value);
    }

    public bool IsFlowActive
    {
        get;
        set => SetValue(ref field, value);
    }

    public string Email
    {
        get;
        set
        {
            CanRequestVerification = field.Length > 0;
            SetValue(ref field, value);
        }
    } = string.Empty;

    public async Task RequestAccountVerification()
    {
        IsRequestingVerification = true;

        PoogieResult<RequestAccountVerificationResponse> response = await accountConnector.RequestAccountVerificationAsync(
            new RequestAccountVerifyRequest(Email: Email)
        );

        IsRequestingVerification = false;

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
                "//Strings/Client/Integrations/Poogie[@Id='ACCOUNT_REGISTER_SUCCESS']"
            ).Replace("{Email}", Email),
            DisplayTime: TimeSpan.FromSeconds(10)
        );
        await NotificationService.Show(successOptions);

        IsFlowActive = false;
        Email = string.Empty;
    }

    public void NavigateToLoginFlow() => IsFlowActive = false;
}