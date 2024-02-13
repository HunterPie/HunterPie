using HunterPie.Core.Crypto;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Game.Quest;
using HunterPie.Core.Game.Events;
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
        if (!AccountManager.IsLoggedIn() || !LocalAccountConfig.Config.IsHuntUploadEnabled)
            return;

        HuntStatisticsModel? exported = _statisticsService?.Export();

        _statisticsService?.Dispose();

        if (e.Status != QuestStatus.Success || !ShouldUpload(exported))
            return;

        DateTime questFinishedAt = exported!.StartedAt.Add(e.TimeElapsed);
        string newHash = await GenerateUniqueHashAsync(questFinishedAt, exported.Hash);

        exported = exported with
        {
            FinishedAt = questFinishedAt,
            Hash = newHash
        };

        var exportedRequest = PoogieQuestStatisticsModel.From(exported);

        await _connector.Upload(exportedRequest)
            .ConfigureAwait(false);
    }

    private void OnQuestStart(object? sender, IQuest e)
    {
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

    private static bool ShouldUpload(HuntStatisticsModel? model)
    {
        return model is { } && model.Monsters.Count > 0;
    }

    public Task InitializeAsync(IContext context)
    {
        _context = context;
        HookEvents();

        return Task.CompletedTask;
    }
}
