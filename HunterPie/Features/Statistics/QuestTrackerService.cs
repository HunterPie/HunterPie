using HunterPie.Core.Client;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Game;
using HunterPie.Core.Json;
using HunterPie.Core.Logger;
using HunterPie.Features.Statistics.Models;
using System;
using System.IO;

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

        _statisticsService?.Dispose();

        if (exported is null)
            return;

        File.WriteAllText(ClientInfo.GetPathFor("test.json"), JsonProvider.Serializer(exported, indented: true));
        Log.Debug("Exported hunt with hash: {0}", exported.Hash);
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
}
