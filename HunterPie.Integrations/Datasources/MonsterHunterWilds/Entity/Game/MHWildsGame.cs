using HunterPie.Core.Address.Map;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Entity.Game.Chat;
using HunterPie.Core.Game.Entity.Game.Quest;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Game.Services;
using HunterPie.Core.Scan.Service;
using HunterPie.Core.Utils;
using HunterPie.Integrations.Datasources.Common.Entity.Game;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Monster;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Quest;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.World;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Crypto;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Game.Quest;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Player;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Utils;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Game;

public sealed class MHWildsGame : CommonGame
{
    private DateTime _localElapsedTime = DateTime.UtcNow;

    private readonly MHWildsCryptoService _cryptoService;
    private readonly ILocalizationRepository _localizationRepository;
    private readonly MHWildsMonsterTargetKeyManager _monsterTargetKeyManager;

    private readonly MHWildsPlayer _player;
    public override IPlayer Player => _player;

    public override IAbnormalityCategorizationService AbnormalityCategorizationService => throw new NotImplementedException();

    private readonly Dictionary<nint, MHWildsMonster> _monsters = new(3);
    public override IReadOnlyCollection<IMonster> Monsters => _monsters.Values;

    public override IChat? Chat => null;

    private bool _isHudOpen;

    public override bool IsHudOpen
    {
        get => _isHudOpen;
        protected set
        {
            if (_isHudOpen == value)
                return;

            _isHudOpen = value;
            this.Dispatch(
                toDispatch: _onHudStateChange,
                data: this
            );
        }
    }

    private float _timeElapsed;
    public override float TimeElapsed
    {
        get => _timeElapsed;
        protected set
        {
            if (_timeElapsed.Equals(value))
                return;

            bool hasReset = Math.Abs(value - _timeElapsed) > 5;

            _timeElapsed = value;
            this.Dispatch(
                toDispatch: _onTimeElapsedChange,
                data: new TimeElapsedChangeEventArgs(hasReset, value)
            );
        }
    }

    private MHWildsQuest? _quest;
    public override IQuest? Quest => _quest;

    public MHWildsGame(
        IGameProcess process,
        IScanService scanService,
        ILocalizationRepository localizationRepository,
        MHWildsMonsterTargetKeyManager monsterTargetKeyManager) : base(process, scanService)
    {
        _localizationRepository = localizationRepository;
        _monsterTargetKeyManager = monsterTargetKeyManager;
        _cryptoService = new MHWildsCryptoService(process.Memory);
        _player = new MHWildsPlayer(
            process: process,
            scanService: scanService,
            monsterTargetKeyManager: _monsterTargetKeyManager
        );
        _player.OnStageUpdate += (_, _) => _localElapsedTime = DateTime.UtcNow;
    }

    [ScannableMethod]
    internal async Task GetHUDStateAsync()
    {
        byte state = await Memory.DerefAsync<byte>(
            address: AddressMap.GetAbsolute("Game::GUIManager"),
            offsets: AddressMap.GetOffsets("GUI::VisibilityFlag")
        );

        IsHudOpen = state > 0;
    }

    [ScannableMethod]
    internal async Task GetMonstersAsync()
    {
        nint monsterArrayPointer = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("Game::EnemyManager"),
            offsets: AddressMap.GetOffsets("Environment::MonsterList")
        );
        nint[] monsters = await Memory.ReadDynamicArraySafeAsync(
            address: monsterArrayPointer,
            count: 700
        );
        nint[] validMonsters = monsters.Where(it => !it.IsNullPointer())
            .ToArray();

        IAsyncEnumerable<(
            nint address,
            int magic,
            MHWildsMonsterBasicData data
        )> monstersToCreate = validMonsters.Where(it => !_monsters.ContainsKey(it))
            .Select(async it => (
                address: it,
                magic: (int)await Memory.ReadPtrAsync(
                    address: it,
                    offsets: AddressMap.GetOffsets("Monster::Magic")
                ),
                data: await Memory.DerefPtrAsync<MHWildsMonsterBasicData>(
                    address: it,
                    offsets: AddressMap.GetOffsets("Monster::BasicData")
                )
            )).ToAsyncEnumerable();

        await foreach ((nint address, int magic, MHWildsMonsterBasicData data) monster in monstersToCreate)
        {
            // 0x6D0045 are just the UTF-16 bytes for "Em", every monster asset starts with Em
            if (monster is not { magic: 0x6D0045, data.IsEnabled: 1, data.Category: 0 })
                continue;

            HandleMonsterSpawn(monster.address, monster.data);
        }

        IEnumerable<nint> monstersToDestroy = _monsters.Keys.Where(it => !validMonsters.Contains(it));

        monstersToDestroy.ForEach(HandleMonsterDespawn);
    }

    [ScannableMethod]
    internal async Task GetWorldTimeAsync()
    {
        nint worldPointer = await Memory.ReadAsync<nint>(
            address: AddressMap.GetAbsolute("Game::WorldManager")
        );

        if (worldPointer.IsNullPointer())
        {
            WorldTime = TimeOnly.MinValue;
            return;
        }

        MHWildsWorldTime worldTime = await Memory.ReadAsync<MHWildsWorldTime>(worldPointer);

        double hours = Math.Floor(worldTime.Current / 100);
        double minutes = Math.Floor(worldTime.Current - (hours * 100));

        WorldTime = new TimeOnly((int)hours % 24, (int)minutes % 60);
    }

    [ScannableMethod]
    internal async Task GetQuestAsync()
    {
        nint questPointer = await Memory.DerefAsync<nint>(
            address: AddressMap.GetAbsolute("Game::QuestManager"),
            offsets: AddressMap.GetOffsets("Quest::Data")
        );
        bool isQuestValid = !questPointer.IsNullPointer();

        if (!isQuestValid && _quest is null)
        {
            TimeElapsed = (float)(DateTime.UtcNow - _localElapsedTime).TotalSeconds;
            return;
        }

        MHWildsQuestInformation quest = await Memory.ReadAsync<MHWildsQuestInformation>(questPointer);

        nint informationPointer = await Memory.DerefAsync<nint>(
            address: AddressMap.GetAbsolute("Game::QuestManager"),
            offsets: AddressMap.GetOffsets("Quest::CurrentInformation")
        );
        MHWildsCurrentQuestInformation information = await Memory.ReadAsync<MHWildsCurrentQuestInformation>(informationPointer);
        bool hasStarted = information is { FailureState: 0, SuccessState: 0 };
        bool isOver = !hasStarted || !isQuestValid;

        if (_quest is not null && isOver)
        {
            _localElapsedTime = DateTime.UtcNow;

            this.Dispatch(
                toDispatch: _onQuestEnd,
                data: new QuestEndEventArgs(
                    quest: _quest,
                    status: information.ToQuestStatus(),
                    timeElapsed: TimeElapsed
                )
            );
            _monsterTargetKeyManager.Clear();
            _quest.Dispose();
            _quest = null;
            return;
        }

        if (_quest is null
            && hasStarted
            && isQuestValid)
        {
            MHWildsTargetKey[] targetKeys = await Memory.ReadArrayAsync<MHWildsTargetKey>(
                address: information.TargetKeysPointer
            );
            MHWildsQuestDetails? details = quest.DetailsPointer.IsNullPointer() switch
            {
                false => await Memory.ReadAsync<MHWildsQuestDetails>(
                    address: quest.DetailsPointer
                ),
                _ => null
            };
            _quest = new MHWildsQuest(
                information: quest,
                details: details
            );
            _quest.Update(information);
            _monsterTargetKeyManager.Set(targetKeys);

            this.Dispatch(
                toDispatch: _onQuestStart,
                data: _quest
            );
            return;
        }

        _quest?.Update(information);

        TimeElapsed = _quest is null
            ? 0.0f
            : information.Timer / 1000;
    }

    private void HandleMonsterSpawn(
        nint address,
        MHWildsMonsterBasicData data)
    {
        var monster = new MHWildsMonster(
            process: Process,
            scanService: ScanService,
            address: address,
            basicData: data,
            cryptoService: _cryptoService,
            localizationRepository: _localizationRepository,
            targetKeyManager: _monsterTargetKeyManager
        );

        _monsters[address] = monster;

        this.Dispatch(
            toDispatch: _onMonsterSpawn,
            data: monster
        );
    }

    private void HandleMonsterDespawn(nint address)
    {
        if (_monsters[address] is not { } monster)
            return;

        _monsters.Remove(address);

        this.Dispatch(
            toDispatch: _onMonsterDespawn,
            data: monster
        );

        monster.Dispose();
    }

    public override void Dispose()
    {
        base.Dispose();
        _cryptoService.Dispose();
    }
}