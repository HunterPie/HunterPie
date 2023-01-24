using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Game;
using HunterPie.Features.Statistics.Models;
using System;

namespace HunterPie.Features.Statistics;

#nullable enable
internal class QuestTrackerService : IDisposable
{
    private readonly IContext _context;

    private HuntStatisticsService? _statisticsService;

    public QuestTrackerService(IContext context)
    {
        _context = context;

        HookEvents();
    }

    private void HookEvents()
    {
        _context.Game.OnQuestStart += OnQuestStart;
        _context.Game.OnQuestEnd += OnQuestEnd;
    }

    private void UnhookEvents()
    {
        _context.Game.OnQuestStart -= OnQuestStart;
        _context.Game.OnQuestEnd -= OnQuestEnd;
    }

    private void OnQuestEnd(object? sender, IGame e)
    {
        HuntStatisticsModel? exported = _statisticsService?.Export();

        if (exported is null)
            return;
    }

    private void OnQuestStart(object? sender, IGame e)
    {
        _statisticsService = new HuntStatisticsService(_context);

        // TODO: Upload quest to remote server
    }

    public void Dispose()
    {
        UnhookEvents();
        _statisticsService?.Dispose();
    }
}
