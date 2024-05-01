using HunterPie.Core.Crypto;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Game.Quest;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Logger;
using HunterPie.Domain.Interfaces;
using HunterPie.Features.Account;
using HunterPie.Features.Account.Config;
using HunterPie.Features.Statistics.Models;
using HunterPie.Integrations.Poogie.Statistics;
using HunterPie.Integrations.Poogie.Statistics.Models;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace HunterPie.Features.Statistics;

#nullable enable
internal class QuestTrackerService : IContextInitializer, IDisposable
{
    private IContext? _context;
    private readonly PoogieStatisticsConnector _connector = new();

    private HuntStatisticsService? _statisticsService;

    private void HookEvents()
    {
        if (_context is null)
            return;

        _context!.Game.OnQuestStart += OnQuestStart;
        _context.Game.OnQuestEnd += OnQuestEnd;
    }

    private void UnhookEvents()
    {
        if (_context is null)
            return;

        _context!.Game.OnQuestStart -= OnQuestStart;
        _context.Game.OnQuestEnd -= OnQuestEnd;
    }

    private async void OnQuestEnd(object? sender, QuestEndEventArgs e)
    {
        Log.Debug("Quest ended with status {0}", e.Status);

        if (_statisticsService is null)
            return;

        if (!AccountManager.IsLoggedIn() || !LocalAccountConfig.Config.IsHuntUploadEnabled)
            return;

        HuntStatisticsModel exported = _statisticsService.Export();

        if (e.Status != QuestStatus.Success || !ShouldUpload(exported))
            return;

        DateTime questFinishedAt = exported.StartedAt.Add(e.TimeElapsed);
        string newHash = await GenerateUniqueHashAsync(questFinishedAt, exported.Hash);

        exported = exported with
        {
            FinishedAt = questFinishedAt,
            Hash = newHash
        };

        var exportedRequest = PoogieQuestStatisticsModel.From(exported);

        await _connector.Upload(exportedRequest)
            .ConfigureAwait(false);

        _statisticsService.Dispose();
    }

    private void OnQuestStart(object? sender, IQuest e)
    {
        Log.Debug("Quest started (id: {0}, type: {1})", e.Id, e.Type);

        if (_context is null)
            return;

        if (e.Type is not QuestType.Hunt or QuestType.Slay or QuestType.Capture)
            return;

        _statisticsService?.Dispose();
        _statisticsService = new HuntStatisticsService(_context);
    }

    public void Dispose()
    {
        UnhookEvents();
        _statisticsService?.Dispose();
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

    public Task InitializeAsync(IContext context)
    {
        _context = context;
        HookEvents();

        return Task.CompletedTask;
    }
}
