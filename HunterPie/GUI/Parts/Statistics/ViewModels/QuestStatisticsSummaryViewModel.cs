using HunterPie.Features.Notification;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Statistics;
using HunterPie.Integrations.Poogie.Statistics.Models;
using HunterPie.UI.Architecture;
using HunterPie.UI.Controls.Notfication;
using System;
using System.Collections.ObjectModel;
using GameType = HunterPie.Core.Client.Configuration.Enums.GameType;

namespace HunterPie.GUI.Parts.Statistics.ViewModels;
public class QuestStatisticsSummaryViewModel : ViewModel
{
    private readonly PoogieStatisticsConnector _connector = new();
    private readonly string _uploadId;

    private GameType _gameType;
    public GameType GameType
    {
        get => _gameType;
        set => SetValue(ref _gameType, value);
    }

    private DateTime _uploadedAt;
    public DateTime UploadedAt
    {
        get => _uploadedAt;
        set => SetValue(ref _uploadedAt, value);
    }

    private bool _isFetchingDetails;
    public bool IsFetchingDetails
    {
        get => _isFetchingDetails;
        set => SetValue(ref _isFetchingDetails, value);
    }

    public ObservableCollection<MonsterSummaryViewModel> Monsters { get; } = new();

    public QuestStatisticsSummaryViewModel() { }

    internal QuestStatisticsSummaryViewModel(PoogieQuestSummaryModel model)
    {
        _uploadId = model.Id;

        GameType = model.GameType.ToEntity();

        UploadedAt = model.CreatedAt.ToLocalTime();

        foreach (PoogieMonsterSummaryModel monster in model.Monsters)
            Monsters.Add(new MonsterSummaryViewModel(model.GameType.ToEntity(), monster));
    }

    public async void NavigateToHuntDetails()
    {
        IsFetchingDetails = true;

        PoogieResult<PoogieQuestStatisticsModel> questResponse = await _connector.Get(_uploadId);

        if (questResponse.Response is not { } questDetails)
        {
            AppNotificationManager.Push(
                Push.Error(
                    "Failed to retrieve quest details. Try again later!"
                ),
                TimeSpan.FromSeconds(10)
            );
            IsFetchingDetails = false;
            return;
        }

        // TODO: Navigate to quest details
        // MainHost.SetMain();
    }
}
