using HunterPie.Core.Client.Localization;
using HunterPie.Core.Extensions;
using HunterPie.Core.Notification;
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HunterPie.GUI.Parts.Statistics.ViewModels;

public class QuestStatisticsSummariesViewModel : ViewModel
{
    private const int MAX_PER_PAGE = 5;
    private PoogieQuestSummaryModel[] _summaries = Array.Empty<PoogieQuestSummaryModel>();
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
        set => SetValue(ref _currentPage, value);
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

    private QuestSupporterTierMessageType _messageType;
    public QuestSupporterTierMessageType MessageType { get => _messageType; set => SetValue(ref _messageType, value); }

    public ObservableCollection<QuestStatisticsSummaryViewModel> Summaries { get; } = new();

    public async void FetchQuests()
    {
        UserAccount? account = await AccountManager.FetchAccount();

        if (account is not { })
            return;

        MessageType = ConvertTierToMessageType(account.Tier);

        IsFetchingQuests = true;

        PoogieResult<List<PoogieQuestSummaryModel>> summariesResponse = await _connector.GetUserQuestSummaries();

        if (summariesResponse.Error is { } error && error.Code != PoogieErrorCode.NOT_ERROR)
        {
            HasFetchingFailed = true;
            return;
        }

        IsFetchingQuests = false;

        if (summariesResponse.Response is not { } summaries || !summaries.Any())
        {
            HasQuests = false;
            return;
        }

        _summaries = summaries.OrderByDescending(it => it.CreatedAt)
            .ToArray();

        LastPage = summaries.Count / MAX_PER_PAGE;
        HasQuests = true;

        UpdateSummariesContainer();
    }

    public void RequestPageUpdate() => UpdateSummariesContainer();

    public async void NavigateToHuntDetails(string uploadId)
    {
        NotificationService.Info(
            Localization.QueryString("//Strings/Client/Main/String[@Id='CLIENT_HUNT_EXPORT_FETCH_IN_PROGRESS_STRING']")
            .Format(uploadId),
            TimeSpan.FromSeconds(5)
        );
        IsFetchingDetails = true;

        PoogieResult<PoogieQuestStatisticsModel> questResponse = await _connector.Get(uploadId);

        if (questResponse.Response is not { } questDetails)
        {
            NotificationService.Error(
                Localization.QueryString("//Strings/Client/Main/String[@Id='CLIENT_HUNT_EXPORT_FETCH_FAILED_ERROR_STRING']"),
                TimeSpan.FromSeconds(10)
            );
            IsFetchingDetails = false;
            return;
        }

        IsFetchingDetails = false;

        NotificationService.Success(
            Localization.QueryString("//Strings/Client/Main/String[@Id='CLIENT_HUNT_EXPORT_FETCH_SUCCESS_STRING']"),
            TimeSpan.FromSeconds(5)
        );

        QuestDetailsViewModel viewModel = await QuestDetailsViewModelBuilder.From(questDetails.ToEntity());
        Navigator.Body.Navigate(viewModel);
    }

    private void UpdateSummariesContainer()
    {
        Summaries.Clear();

        foreach (PoogieQuestSummaryModel summary in _summaries.Skip(CurrentPage * MAX_PER_PAGE).Take(MAX_PER_PAGE))
            Summaries.Add(new QuestStatisticsSummaryViewModel(summary));
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