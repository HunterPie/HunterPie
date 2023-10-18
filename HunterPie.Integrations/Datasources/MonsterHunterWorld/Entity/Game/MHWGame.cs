using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Entity.Game.Chat;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Game.Services;
using HunterPie.Core.Native.IPC.Handlers.Internal.Damage;
using HunterPie.Core.Native.IPC.Handlers.Internal.Damage.Models;
using HunterPie.Core.Native.IPC.Models.Common;
using HunterPie.Integrations.Datasources.Common;
using HunterPie.Integrations.Datasources.Common.Entity.Game;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Crypto;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Enums;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Party;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Services;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Utils;
using System.Diagnostics;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Game;

public sealed class MHWGame : CommonGame
{
    private readonly MHWPlayer _player;
    private readonly Dictionary<long, IMonster> _monsters = new();
    private readonly Dictionary<long, EntityDamageData[]> _damageDone = new();
    private bool _isMouseVisible;
    private int _deaths;
    private bool _isInQuest;
    private readonly Stopwatch _localTimerStopwatch = new();
    private readonly Stopwatch _damageUpdateThrottleStopwatch = new();

    public override IPlayer Player => _player;
    public override List<IMonster> Monsters { get; } = new();

    public override IChat Chat => null;

    public override bool IsHudOpen
    {
        get => _isMouseVisible;
        protected set
        {
            if (value != _isMouseVisible)
            {
                _isMouseVisible = value;
                this.Dispatch(_onHudStateChange, this);
            }
        }
    }

    public override float TimeElapsed { get; protected set; }

    private void SetTimeElapsed(float value, bool isReset)
    {
        if (value == TimeElapsed)
            return;

        TimeElapsed = value;
        this.Dispatch(_onTimeElapsedChange, new TimeElapsedChangeEventArgs(isReset, value));
    }

    public override int MaxDeaths
    {
        get => 0;
        protected set => throw new NotSupportedException();
    }

    public override int Deaths
    {
        get => _deaths;
        protected set
        {
            if (value != _deaths)
            {
                _deaths = value;
                this.Dispatch(_onDeathCountChange, this);
            }
        }
    }

    public override bool IsInQuest
    {
        get => _isInQuest;
        protected set
        {
            if (value != _isInQuest)
            {
                _isInQuest = value;
                this.Dispatch(value ? _onQuestStart : _onQuestEnd, new QuestStateChangeEventArgs(this));
            }
        }
    }

    public override IAbnormalityCategorizationService AbnormalityCategorizationService { get; } = new MHWAbnormalityCategorizatonService();

    public MHWGame(IProcessManager process) : base(process)
    {
        _player = new(process);
        DamageMessageHandler.OnReceived += OnReceivePlayersDamage;
        _player.OnStageUpdate += OnPlayerStageUpdate;

        ScanManager.Add(_player, this);
    }

    [ScannableMethod]
    private void GetMouseVisibilityState()
    {
        bool isMouseVisible = Process.Memory.Deref<int>(
            AddressMap.GetAbsolute("GAME_MOUSE_INFO_ADDRESS"),
            AddressMap.Get<int[]>("MOUSE_VISIBILITY_OFFSETS")
        ) == 1;

        IsHudOpen = isMouseVisible;
    }

    [ScannableMethod]
    private void GetTimeElapsed()
    {
        long questEndTimerPtrs = Process.Memory.Read(
            AddressMap.GetAbsolute("QUEST_DATA_ADDRESS"),
            AddressMap.Get<int[]>("QUEST_TIMER_OFFSETS")
        );
        ulong timer = Process.Memory.Read<ulong>(questEndTimerPtrs);
        uint questMaxTimerRaw = Process.Memory.Read<uint>(questEndTimerPtrs + 0x10);

        float elapsed = MHWCrypto.LiterallyWhyCapcom(timer);

        if (QuestStatus == QuestStatus.None)
        {
            if (!Player.InHuntingZone)
            {
                SetTimeElapsed(0, false);
                return;
            }

            if (!_localTimerStopwatch.IsRunning)
            {
                _localTimerStopwatch.Start();
                SetTimeElapsed(0, true);
                return;
            }

            SetTimeElapsed(_localTimerStopwatch.ElapsedMilliseconds / 1000.0f, false);

            return;
        }

        float questMaxTimer = questMaxTimerRaw
                .ApproximateHigh(MHWGameUtils.MaxQuestTimers)
                .ToSeconds();

        float timeElapsed = Math.Max(0, questMaxTimer - elapsed);

        if (!_localTimerStopwatch.IsRunning)
        {
            SetTimeElapsed(timeElapsed, TimeElapsed == 0);
            return;
        }

        _localTimerStopwatch.Reset();
        SetTimeElapsed(timeElapsed, true);
    }

    [ScannableMethod]
    private void GetQuestState()
    {
        var questState = (QuestState)Memory.Deref<int>(
            AddressMap.GetAbsolute("QUEST_DATA_ADDRESS"),
            AddressMap.Get<int[]>("QUEST_STATE_OFFSETS")
        );

        QuestStatus = questState.ToStatus();

        IsInQuest = questState == QuestState.InQuest;
    }

    [ScannableMethod]
    private void GetPartyMembersDamage()
    {
        if (_damageUpdateThrottleStopwatch.IsRunning && _damageUpdateThrottleStopwatch.ElapsedMilliseconds < 100)
            return;

        _damageUpdateThrottleStopwatch.Restart();

        if (Player.InHuntingZone)
            DamageMessageHandler.RequestHuntStatistics(CommonConstants.AllTargets);
    }

    [ScannableMethod]
    private void GetDeathCounter()
    {
        int deathCounter = Process.Memory.Deref<int>(
            AddressMap.GetAbsolute("QUEST_DATA_ADDRESS"),
            AddressMap.Get<int[]>("QUEST_DEATH_COUNTER_OFFSETS")
        );

        Deaths = deathCounter;
    }

    [ScannableMethod]
    private void GetMonsterDoubleLinkedList()
    {
        long doubleLinkedListHead = Process.Memory.Read(
            AddressMap.GetAbsolute("MONSTER_ADDRESS"),
            AddressMap.Get<int[]>("MONSTER_OFFSETS")
        );

        long next = doubleLinkedListHead;
        bool isBigMonster;
        HashSet<long> monsterAddresses = new();
        do
        {
            long monsterEmPtr = Process.Memory.Read<long>(next + 0x2A0);
            string monsterEm = Process.Memory.Read(monsterEmPtr + 0x0C, 64);

            isBigMonster = monsterEmPtr != 0
                && monsterEm.StartsWith("em\\em")
                && !monsterEm.StartsWith("em\\ems");

            if (!isBigMonster)
                break;

            _ = monsterAddresses.Add(next);

            string? em = monsterEm.Split('\\')
                                  .ElementAtOrDefault(1);

            if (em is null)
                break;

            HandleMonsterSpawn(next, em);

            next = Process.Memory.Read<long>(next - 0x30) + 0x40;
        } while (isBigMonster);

        long[] toDespawn = _monsters.Keys.Where(address => !monsterAddresses.Contains(address))
            .ToArray();

        foreach (long monsterAddress in toDespawn)
            HandleMonsterDespawn(monsterAddress);
    }

    private void HandleMonsterSpawn(long address, string em)
    {
        if (_monsters.ContainsKey(address))
            return;

        var monster = new MHWMonster(Process, address, em);
        _monsters.Add(address, monster);
        Monsters.Add(monster);
        ScanManager.Add(monster);

        this.Dispatch(_onMonsterSpawn, monster);
    }

    private void HandleMonsterDespawn(long address)
    {
        if (_monsters[address] is not MHWMonster monster)
            return;

        _ = _monsters.Remove(address);
        _ = Monsters.Remove(monster);
        ScanManager.Remove(monster);

        this.Dispatch(_onMonsterDespawn, monster);

        monster.Dispose();
    }

    public override void Dispose()
    {
        DamageMessageHandler.OnReceived -= OnReceivePlayersDamage;
        _player.OnStageUpdate -= OnPlayerStageUpdate;
        base.Dispose();
    }

    #region Damage helpers

    private void OnPlayerStageUpdate(object? sender, EventArgs e)
    {
        if (!Player.InHuntingZone)
            // When back from hunt, manually clear damage data in case player enters Training Area (data won't be reset)
            foreach (MHWPartyMember member in Player.Party.Members.Cast<MHWPartyMember>())
                member.ResetDamage();

        DamageMessageHandler.ClearAllHuntStatisticsExcept(Array.Empty<long>());
        DamageMessageHandler.RequestHuntStatistics(CommonConstants.AllTargets);
        _damageUpdateThrottleStopwatch.Reset();
        _localTimerStopwatch.Reset();
    }

    private void OnReceivePlayersDamage(object? sender, ResponseDamageMessage e)
    {
        long target = e.Target;

        _damageDone[target] = e.Entities;

        EntityDamageData[] damages = _damageDone.Values.SelectMany(entity => entity)
            .GroupBy(entity => entity.Entity.Index)
            .Select(group =>
            {
                EntityDamageData entity = group.ElementAt(0);

                return new EntityDamageData
                {
                    Target = entity.Target,
                    Entity = entity.Entity,
                    RawDamage = group.Sum(e => e.RawDamage),
                    ElementalDamage = group.Sum(e => e.ElementalDamage)
                };
            })
            .ToArray();

        _player.UpdatePartyMembersDamage(damages);
    }
    #endregion
}