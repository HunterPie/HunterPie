using HunterPie.Core.Crypto;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Entity.Party;
using HunterPie.Core.Game.Enums;
using HunterPie.Features.Statistics.Interfaces;
using HunterPie.Features.Statistics.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.Features.Statistics;

internal class HuntStatisticsService : IHuntStatisticsService<HuntStatisticsModel>
{
    private readonly IContext _context;
    private readonly DateTime _startedAt = DateTime.UtcNow;
    private readonly List<IHuntStatisticsService<MonsterModel>> _monsterStatisticsServices = new();
    private readonly List<IHuntStatisticsService<PartyMemberModel>> _partyMembersStatisticsServices = new();

    public HuntStatisticsService(IContext context)
    {
        _context = context;

        HookEvents();
    }

    public HuntStatisticsModel Export()
    {
        var players = _partyMembersStatisticsServices.Select(s => s.Export())
            .ToList();

        var monsters = _monsterStatisticsServices.Select(s => s.Export())
            .ToList();

        return new HuntStatisticsModel(
            Players: players,
            Monsters: monsters,
            StartedAt: _startedAt,
            FinishedAt: DateTime.UtcNow,
            Hash: GenerateHuntHash(players, monsters)
        );
    }

    private void HookEvents()
    {
        _context.Game.Player.Party.OnMemberJoin += OnPartyMemberJoin;
        _context.Game.OnMonsterSpawn += OnMonsterSpawn;
    }

    private void UnhookEvents()
    {
        _context.Game.Player.Party.OnMemberJoin -= OnPartyMemberJoin;
        _context.Game.OnMonsterSpawn -= OnMonsterSpawn;
    }

    private void OnMonsterSpawn(object sender, IMonster e)
    {
        _monsterStatisticsServices.Add(new MonsterStatisticsService(_context, e));
    }

    private void OnPartyMemberJoin(object sender, IPartyMember e)
    {
        if (e.Type != MemberType.Player)
            return;

        _partyMembersStatisticsServices.Add(new PartyMemberStatisticsService(_context, e));
    }

    private string GenerateHuntHash(List<PartyMemberModel> players, List<MonsterModel> monsters)
    {
        string[] data = players.Select(p => p.Name)
            .Concat(monsters.Select(m => m.Id.ToString()))
            .Where(e => !string.IsNullOrEmpty(e))
            .OrderBy(e => e.Length)
            .ToArray();

        string uniqueString = string.Join('|', data);

        return HashService.Hash(uniqueString);
    }

    public void Dispose()
    {
        UnhookEvents();

        _monsterStatisticsServices.DisposeAll();
        _monsterStatisticsServices.Clear();

        _partyMembersStatisticsServices.DisposeAll();
        _partyMembersStatisticsServices.Clear();
    }
}
