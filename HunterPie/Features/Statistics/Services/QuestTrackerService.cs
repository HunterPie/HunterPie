using HunterPie.Core.Crypto;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Game.Quest;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Observability.Logging;
using HunterPie.Features.Account.Config;
using HunterPie.Features.Account.UseCase;
using HunterPie.Features.Statistics.Models;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Statistics;
using HunterPie.Integrations.Poogie.Statistics.Models;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace HunterPie.Features.Statistics.Services;

internal class QuestTrackerService(
    IContext context,
    PoogieStatisticsConnector connector,
    IAccountUseCase accountUseCase,
    AccountConfig accountConfig
) : IDisposable
{
    private readonly ILogger _logger = LoggerFactory.Create();

    private HuntStatisticsService? statisticsService;

    public void Setup()
    {
        context.Game.OnQuestStart += OnQuestStart;
        context.Game.OnQuestEnd += OnQuestEnd;
    }

    private void UnhookEvents()
    {
        context.Game.OnQuestStart -= OnQuestStart;
        context.Game.OnQuestEnd -= OnQuestEnd;
    }

    private async void OnQuestEnd(object? sender, QuestEndEventArgs e)
    {
        _logger.Debug($"Quest ended with status {e.Status}");

        if (statisticsService is null)
            return;

        if (!await accountUseCase.IsValidSessionAsync() || !accountConfig.IsHuntUploadEnabled)
            return;

        HuntStatisticsModel exported = statisticsService.Export();

        if (e.Status != QuestStatus.Success || !ShouldUpload(exported))
        {
            _logger.Debug($"Quest not uploaded (status: {e.Status}, monsters: {exported.Monsters.Count})");
            return;
        }

        DateTime questFinishedAt = exported.StartedAt.Add(e.TimeElapsed);
        string newHash = await GenerateUniqueHashAsync(questFinishedAt, exported.Hash);

        exported = exported with
        {
            FinishedAt = questFinishedAt,
            Hash = newHash
        };

        var exportedRequest = PoogieQuestStatisticsModel.From(exported);

        PoogieResult<PoogieQuestStatisticsModel> result = await connector.UploadAsync(exportedRequest)
            .ConfigureAwait(false);

        if (result.Error is not { })
            _logger.Debug("Quest uploaded successfully");

        statisticsService.Dispose();
    }

    private void OnQuestStart(object? sender, IQuest e)
    {
        if (context is null)
            return;

        bool shouldIgnore = e.Type switch
        {
            QuestType.Hunt
                or QuestType.Slay
                or QuestType.Capture
                or QuestType.Special => false,
            _ => true
        };

        if (shouldIgnore)
            return;

        _logger.Debug($"Quest started (id: {e.Id}, type: {e.Type})");

        statisticsService?.Dispose();
        statisticsService = new HuntStatisticsService(context);
    }

    public void Dispose()
    {
        UnhookEvents();
        statisticsService?.Dispose();
    }

    private static async Task<string> GenerateUniqueHashAsync(DateTime questFinishedAt, string currentHash)
    {
        string questTimeFormatted = questFinishedAt.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
        return await HashService.HashAsync($"{currentHash}:{questTimeFormatted}");
    }

    private static bool ShouldUpload(HuntStatisticsModel model)
    {
        return model.Monsters.Count > 0;
    }
}