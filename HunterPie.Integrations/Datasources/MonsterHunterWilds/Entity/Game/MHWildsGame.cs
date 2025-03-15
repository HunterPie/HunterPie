using HunterPie.Core.Address.Map;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Entity.Game.Chat;
using HunterPie.Core.Game.Entity.Game.Quest;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Services;
using HunterPie.Core.Scan.Service;
using HunterPie.Core.Utils;
using HunterPie.Integrations.Datasources.Common.Entity.Game;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Monster;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Crypto;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Player;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Utils;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Game;

public sealed class MHWildsGame : CommonGame
{
    private readonly MHWildsCryptoService _cryptoService;
    private readonly ILocalizationRepository _localizationRepository;


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

    public override float TimeElapsed { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

    public override IQuest? Quest => null;

    public MHWildsGame(
        IGameProcess process,
        IScanService scanService,
        ILocalizationRepository localizationRepository) : base(process, scanService)
    {
        _localizationRepository = localizationRepository;
        _cryptoService = new MHWildsCryptoService(process.Memory);
        _player = new MHWildsPlayer(
            process: process,
            scanService: scanService
        );
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

        IAsyncEnumerable<(nint address, int magic, MHWildsMonsterBasicData data)> monstersToCreate = validMonsters.Where(it => !_monsters.ContainsKey(it))
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

    private void HandleMonsterSpawn(nint address, MHWildsMonsterBasicData data)
    {
        var monster = new MHWildsMonster(
            process: Process,
            scanService: ScanService,
            address: address,
            basicData: data,
            cryptoService: _cryptoService,
            _localizationRepository
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