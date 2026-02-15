using HunterPie.Core.Architecture.Collections;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Extensions;
using HunterPie.Core.Notification;
using HunterPie.Core.Notification.Model;
using HunterPie.Features.Account.Model;
using HunterPie.Features.Account.UseCase;
using HunterPie.Features.Statistics.Details.Builders;
using HunterPie.Features.Statistics.Details.ViewModels;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Statistics;
using HunterPie.Integrations.Poogie.Statistics.Models;
using HunterPie.UI.Architecture;
using HunterPie.UI.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace HunterPie.Features.Statistics.ViewModels;

internal class QuestStatisticsSummariesViewModel(
    PoogieStatisticsConnector connector,
    IAccountUseCase accountUseCase,
    IBodyNavigator bodyNavigator,
    QuestDetailsViewModelBuilder questDetailsViewModelBuilder,
    ILocalizationRepository localizationRepository) : ViewModel
{
    private readonly PoogieStatisticsConnector _connector = connector;
    private readonly IAccountUseCase _accountUseCase = accountUseCase;
    private readonly IBodyNavigator _bodyNavigator = bodyNavigator;
    private readonly QuestDetailsViewModelBuilder _questDetailsViewModelBuilder = questDetailsViewModelBuilder;

    public bool HasQuests
    {
        get;
        set => SetValue(ref field, value);
    }
    public bool IsFetchingQuests
    {
        get;
        set => SetValue(ref field, value);
    }
    public bool HasFetchingFailed
    {
        get;
        set => SetValue(ref field, value);
    }
    public int CurrentPage
    {
        get;
        set => SetValueThenExecute(ref field, value, FetchQuests);
    }
    public int LastPage
    {
        get;
        set => SetValue(ref field, value);
    }
    public bool IsFetchingDetails
    {
        get;
        set => SetValue(ref field, value);
    }

    public ObservableCollection<int> PageLimitSizes { get; } = new() { 10, 20, 30, 40, 50 };
    public int LimitSize { get; set => SetValue(ref field, value); } = 10;

    public QuestSupporterTierMessageType MessageType { get; set => SetValue(ref field, value); }

    public ObservableCollectionRange<QuestStatisticsSummaryViewModel> Summaries { get; } = new();

    public async void FetchQuests()
    {
        if (IsFetchingQuests)
            return;

        UserAccount? account = await _accountUseCase.GetAsync();

        if (account is not { })
            return;

        MessageType = ConvertTierToMessageType(account.Tier);

        IsFetchingQuests = true;

        PoogieResult<Paginated<PoogieQuestSummaryModel>> summariesResponse =
            await _connector.GetUserQuestSummariesV2(CurrentPage, LimitSize);

        if (summariesResponse.Error is { } error && error.Code != PoogieErrorCode.NOT_ERROR)
        {
            HasFetchingFailed = true;
            return;
        }

        IsFetchingQuests = false;

        if (summariesResponse.Response is not { } summaries || !summaries.Elements.Any())
        {
            HasQuests = false;
            return;
        }

        Summaries.Replace(
            collection: summaries.Elements.Select(it => new QuestStatisticsSummaryViewModel(
                model: it,
                localizationRepository: localizationRepository
            ))
        );

        LastPage = summaries.TotalPages;
        HasQuests = true;
    }

    public async void NavigateToHuntDetails(string uploadId)
    {
        var downloadingNotificationOptions = new NotificationOptions(
            Type: NotificationType.InProgress,
            Title: "Quest",
            Description: localizationRepository.FindStringBy("//Strings/Client/Main/String[@Id='CLIENT_HUNT_EXPORT_FETCH_IN_PROGRESS_STRING']")
                .Format(uploadId),
            DisplayTime: TimeSpan.FromSeconds(10)
        );
        Guid notificationId = await NotificationService.Show(downloadingNotificationOptions);
        IsFetchingDetails = true;

        PoogieResult<PoogieQuestStatisticsModel> questResponse = await _connector.GetAsync(uploadId);

        if (questResponse.Response is not { } questDetails)
        {
            NotificationOptions failedNotification = downloadingNotificationOptions with
            {
                Type = NotificationType.Error,
                Description = localizationRepository.FindStringBy("//Strings/Client/Main/String[@Id='CLIENT_HUNT_EXPORT_FETCH_FAILED_ERROR_STRING']")
            };
            NotificationService.Update(notificationId, failedNotification);
            IsFetchingDetails = false;
            return;
        }

        IsFetchingDetails = false;

        NotificationOptions successNotification = downloadingNotificationOptions with
        {
            Type = NotificationType.Success,
            Description = localizationRepository.FindStringBy("//Strings/Client/Main/String[@Id='CLIENT_HUNT_EXPORT_FETCH_SUCCESS_STRING']")
        };
        NotificationService.Update(notificationId, successNotification);

        QuestDetailsViewModel viewModel = await _questDetailsViewModelBuilder.From(questDetails.ToEntity());
        _bodyNavigator.Navigate(viewModel);
    }

    private static QuestSupporterTierMessageType ConvertTierToMessageType(AccountTier tier) =>
        tier switch
        {
            >= AccountTier.HighRank => QuestSupporterTierMessageType.NoTierMessage,
            >= AccountTier.LowRank => QuestSupporterTierMessageType.HighTierMessage,
            >= AccountTier.None => QuestSupporterTierMessageType.LowTierMessage,
            _ => throw new ArgumentOutOfRangeException(nameof(tier), tier, null)
        };
}