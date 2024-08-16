using HunterPie.Core.Architecture.Collections;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Extensions;
using HunterPie.Core.Notification;
using HunterPie.Core.Notification.Model;
using HunterPie.Features.Account;
using HunterPie.Features.Account.Model;
using HunterPie.GUI.Parts.Statistics.Details.Builders;
using HunterPie.GUI.Parts.Statistics.Details.ViewModels;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Statistics;
using HunterPie.Integrations.Poogie.Statistics.Models;
using HunterPie.UI.Architecture;
using HunterPie.UI.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace HunterPie.GUI.Parts.Statistics.ViewModels;

public class QuestStatisticsSummariesViewModel : ViewModel
{
    private readonly PoogieStatisticsConnector _connector = new();

    private bool _hasQuests;
    public bool HasQuests
    {
        get => _hasQuests;
        set => SetValue(ref _hasQuests, value);
    }

    private bool _isFetchingQuests;
    public bool IsFetchingQuests
    {
        get => _isFetchingQuests;
        set => SetValue(ref _isFetchingQuests, value);
    }

    private bool _hasFetchingFailed;
    public bool HasFetchingFailed
    {
        get => _hasFetchingFailed;
        set => SetValue(ref _hasFetchingFailed, value);
    }

    private int _currentPage;
    public int CurrentPage
    {
        get => _currentPage;
        set => SetValueThenExecute(ref _currentPage, value, FetchQuests);
    }

    private int _lastPage;
    public int LastPage
    {
        get => _lastPage;
        set => SetValue(ref _lastPage, value);
    }

    private bool _isFetchingDetails;
    public bool IsFetchingDetails
    {
        get => _isFetchingDetails;
        set => SetValue(ref _isFetchingDetails, value);
    }

    public ObservableCollection<int> PageLimitSizes { get; } = new() { 10, 20, 30, 40, 50 };

    private int _limitSize = 10;
    public int LimitSize { get => _limitSize; set => SetValue(ref _limitSize, value); }

    private QuestSupporterTierMessageType _messageType;
    public QuestSupporterTierMessageType MessageType { get => _messageType; set => SetValue(ref _messageType, value); }

    public ObservableCollectionRange<QuestStatisticsSummaryViewModel> Summaries { get; } = new();

    public async void FetchQuests()
    {
        UserAccount? account = await AccountManager.FetchAccount();

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
            collection: summaries.Elements.Select(it => new QuestStatisticsSummaryViewModel(it))
        );

        LastPage = summaries.TotalPages;
        HasQuests = true;
    }

    public async void NavigateToHuntDetails(string uploadId)
    {
        var downloadingNotificationOptions = new NotificationOptions(
            Type: NotificationType.InProgress,
            Title: "Quest",
            Description: Localization.QueryString("//Strings/Client/Main/String[@Id='CLIENT_HUNT_EXPORT_FETCH_IN_PROGRESS_STRING']")
                .Format(uploadId),
            DisplayTime: TimeSpan.FromSeconds(10)
        );
        Guid notificationId = await NotificationService.Show(downloadingNotificationOptions);
        IsFetchingDetails = true;

        PoogieResult<PoogieQuestStatisticsModel> questResponse = await _connector.Get(uploadId);

        if (questResponse.Response is not { } questDetails)
        {
            NotificationOptions failedNotification = downloadingNotificationOptions with
            {
                Type = NotificationType.Error,
                Description = Localization.QueryString("//Strings/Client/Main/String[@Id='CLIENT_HUNT_EXPORT_FETCH_FAILED_ERROR_STRING']")
            };
            NotificationService.Update(notificationId, failedNotification);
            IsFetchingDetails = false;
            return;
        }

        IsFetchingDetails = false;

        NotificationOptions successNotification = downloadingNotificationOptions with
        {
            Type = NotificationType.Success,
            Description = Localization.QueryString("//Strings/Client/Main/String[@Id='CLIENT_HUNT_EXPORT_FETCH_SUCCESS_STRING']")
        };
        NotificationService.Update(notificationId, successNotification);

        QuestDetailsViewModel viewModel = await QuestDetailsViewModelBuilder.From(questDetails.ToEntity());
        Navigator.Body.Navigate(viewModel);
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