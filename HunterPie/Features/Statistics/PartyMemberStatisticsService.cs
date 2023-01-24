using HunterPie.Core.Extensions;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Party;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Features.Statistics.Interfaces;
using HunterPie.Features.Statistics.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.Features.Statistics;

#nullable enable
internal class PartyMemberStatisticsService : IHuntStatisticsService<PartyMemberModel>
{
    private readonly DateTime _startedAt = DateTime.UtcNow;
    private readonly IPartyMember _partyMember;
    private readonly IContext _context;
    private readonly string _name;
    private readonly List<PlayerDamageFrameModel> _damages = new();
    private readonly Dictionary<string, Stack<TimeFrameModel>> _abnormalities = new();

    private float _currentDamage;
    private Weapon _weapon;


    public PartyMemberStatisticsService(IContext context, IPartyMember partyMember)
    {
        _partyMember = partyMember;
        _context = context;
        _name = partyMember.Name;

        HookEvents();
    }

    public PartyMemberModel Export() => new(
        Name: _name,
        Weapon: _weapon,
        Damages: _damages.ToArray(),
        Abnormalities: _abnormalities.Select(pair => new AbnormalityModel(Id: pair.Key, Activations: pair.Value.ToArray())).ToArray(),
        IsHunterPieUser: _partyMember.IsMyself
    );

    private void HookEvents()
    {
        _partyMember.OnDamageDealt += OnDamageDealt;
        _partyMember.OnWeaponChange += OnWeaponChange;

        if (!_partyMember.IsMyself)
            return;

        _context.Game.Player.OnWeaponChange += OnWeaponChange;
        _context.Game.Player.OnAbnormalityStart += OnAbnormalityStart;
        _context.Game.Player.OnAbnormalityEnd += OnAbnormalityEnd;
    }

    private void UnhookEvents()
    {
        _partyMember.OnDamageDealt -= OnDamageDealt;
        _partyMember.OnWeaponChange -= OnWeaponChange;

        if (!_partyMember.IsMyself)
            return;

        _context.Game.Player.OnWeaponChange -= OnWeaponChange;
        _context.Game.Player.OnAbnormalityStart -= OnAbnormalityStart;
        _context.Game.Player.OnAbnormalityEnd -= OnAbnormalityEnd;
    }

    private void OnAbnormalityEnd(object? sender, IAbnormality e)
    {
        if (!_abnormalities.ContainsKey(e.Id))
            return;

        Stack<TimeFrameModel> timeFrames = _abnormalities[e.Id];

        TimeFrameModel? lastOccurrence = timeFrames.PopOrDefault();
        timeFrames.PushNotNull(lastOccurrence?.End(_startedAt));
    }

    private void OnAbnormalityStart(object? sender, IAbnormality e)
    {
        if (!_abnormalities.ContainsKey(e.Id))
            _abnormalities[e.Id] = new Stack<TimeFrameModel>();

        Stack<TimeFrameModel> timeFrames = _abnormalities[e.Id];

        timeFrames.Push(TimeFrameModel.Start(_startedAt));
    }

    private void OnWeaponChange(object? sender, WeaponChangeEventArgs e) => _weapon = e.NewWeapon.Id;

    private void OnWeaponChange(object? sender, IPartyMember e) => _weapon = e.Weapon;

    private void OnDamageDealt(object? sender, IPartyMember e)
    {
        float hitDamage = e.Damage - _currentDamage;
        DateTime now = DateTime.UtcNow;

        _damages.Add(new PlayerDamageFrameModel(Damage: hitDamage, DealtAt: now));

        _currentDamage = e.Damage;
    }

    public void Dispose() => UnhookEvents();
}
