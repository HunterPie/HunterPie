using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Game;
using HunterPie.Domain.Interfaces;
using HunterPie.Features.Statistics.Models;
using HunterPie.Integrations.Poogie.Statistics;
using HunterPie.Integrations.Poogie.Statistics.Models;
using System;
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
        _context!.Game.OnQuestStart += OnQuestStart;
        _context.Game.OnQuestEnd += OnQuestEnd;
    }

    private void UnhookEvents()
    {
        _context!.Game.OnQuestStart -= OnQuestStart;
        _context.Game.OnQuestEnd -= OnQuestEnd;
    }

    private async void OnQuestEnd(object? sender, IGame e)
    {
        HuntStatisticsModel? exported = _statisticsService?.Export();

        _statisticsService?.Dispose();

        if (exported is null)
            return;

        var exportedRequest = PoogieQuestStatisticsModel.From(exported);

        await _connector.Upload(exportedRequest)
            .ConfigureAwait(false);
    }

    private void OnQuestStart(object? sender, IGame e)
    {
        _statisticsService = new HuntStatisticsService(_context);
    }

    public void Dispose()
    {
        UnhookEvents();
        _statisticsService?.Dispose();
    }

    public Task InitializeAsync(IContext context)
    {
        _context = context;

        return Task.CompletedTask;
    }
}
