using HunterPie.Core.Address.Map;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Core.Game.Data.Repository;
using HunterPie.Core.Game.Entity.Party;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Entity.Player.Vitals;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Game.Services;
using HunterPie.Core.Scan.Service;
using HunterPie.Integrations.Datasources.Common.Entity.Player;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Abnormality;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Player;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Abnormalities;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Abnormalities.Data;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Player.Weapons;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Utils;
using WeaponType = HunterPie.Core.Game.Enums.Weapon;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Player;

public sealed class MHWildsPlayer : CommonPlayer
{
    private static readonly Lazy<AbnormalityDefinition[]> _consumableDefinitions = new(static () =>
        AbnormalityRepository.FindAllAbnormalitiesBy(
            game: GameType.Wilds,
            category: AbnormalityGroup.CONSUMABLES
        )
    );
    private static readonly Lazy<AbnormalityDefinition[]> _songsDefinitions = new(static () =>
        AbnormalityRepository.FindAllAbnormalitiesBy(
            game: GameType.Wilds,
            category: AbnormalityGroup.SONGS
        )
    );
    private static readonly Lazy<AbnormalityDefinition[]> _debuffDefinitions = new(static () =>
        AbnormalityRepository.FindAllAbnormalitiesBy(
            game: GameType.Wilds,
            category: AbnormalityGroup.DEBUFFS
        )
    );
    private static readonly Lazy<int> _debuffIndexMax = new(static () => _debuffDefinitions.Value.Max(it => it.Index));

    private nint _address;

    private string _name = string.Empty;
    public override string Name
    {
        get => _name;
        protected set
        {
            if (value == _name)
                return;

            _name = value;

            this.Dispatch(
                value != ""
                    ? _onLogin
                    : _onLogout
            );
        }
    }

    private int _highRank;
    public override int HighRank
    {
        get => _highRank;
        protected set
        {
            if (value == _highRank)
                return;

            _highRank = value;
            this.Dispatch(
                toDispatch: _onLevelChange,
                data: new LevelChangeEventArgs(this)
            );
        }
    }

    private int _masterRank;
    public override int MasterRank
    {
        get => _masterRank;
        protected set
        {
            if (value == _masterRank)
                return;

            _masterRank = value;
            this.Dispatch(
                toDispatch: _onLevelChange,
                data: new LevelChangeEventArgs(this)
            );
        }
    }

    private int _stageId;
    public override int StageId
    {
        get => _stageId;
        protected set
        {
            if (value == _stageId)
                return;

            _stageId = value;
            this.Dispatch(
                toDispatch: _onStageUpdate,
                data: EventArgs.Empty
            );
        }
    }

    private bool _inHuntingZone;
    public override bool InHuntingZone => _inHuntingZone && StageId > 0;

    public override IParty Party { get; }

    private readonly Dictionary<string, IAbnormality> _abnormalities = new();
    public override IReadOnlyCollection<IAbnormality> Abnormalities => _abnormalities.Values;

    public override IHealthComponent Health { get; }
    public override IStaminaComponent Stamina { get; }

    private IWeapon _weapon;
    public override IWeapon Weapon
    {
        get => _weapon;
        protected set
        {
            if (_weapon == value)
                return;

            IWeapon oldWeapon = _weapon;
            _weapon = value;
            this.Dispatch(
                toDispatch: _onWeaponChange,
                data: new WeaponChangeEventArgs(oldWeapon, value
                )
            );
        }
    }

    public MHWildsPlayer(
        IGameProcess process,
        IScanService scanService) : base(process, scanService)
    {
        Weapon = new MHWildsWeapon(WeaponType.Greatsword);
    }

    [ScannableMethod]
    internal async Task GetBasicDataAsync()
    {
        _address = await Memory.DerefAsync<nint>(
            address: AddressMap.GetAbsolute("Game::PlayerManager"),
            offsets: AddressMap.GetOffsets("Player::Local")
        );

        MHWildsPlayerContext context = await Memory.DerefPtrAsync<MHWildsPlayerContext>(
            address: _address,
            offsets: AddressMap.GetOffsets("Player::Context")
        );

        int hunterRank = await Memory.DerefAsync<int>(
            address: AddressMap.GetAbsolute("Game::SaveManager"),
            offsets: AddressMap.GetOffsets("Save::Player::HunterRank")
        );

        Name = await Memory.ReadStringSafeAsync(context.NamePointer, size: 64);
        HighRank = hunterRank;
    }

    [ScannableMethod]
    internal async Task GetStageAsync()
    {
        MHWildsStageContext context = await Memory.DerefPtrAsync<MHWildsStageContext>(
            address: _address,
            offsets: AddressMap.GetOffsets("Player::Stage")
        );

        StageId = context.StageId;
        _inHuntingZone = !context.IsSafeZone;
    }

    [ScannableMethod]
    internal async Task GetWeaponAsync()
    {
        MHWildsPlayerGearContext context = await Memory.DerefPtrAsync<MHWildsPlayerGearContext>(
            address: _address,
            offsets: AddressMap.GetOffsets("Player::Gear")
        );

        if (Weapon.Id != context.WeaponId)
            Weapon = new MHWildsWeapon(context.WeaponId);
    }

    [ScannableMethod]
    internal Task GetAbnormalitiesCleanUpAsync()
    {
        if (InHuntingZone)
            return Task.CompletedTask;

        ClearAbnormalities(_abnormalities);

        return Task.CompletedTask;
    }

    [ScannableMethod]
    internal async Task GetConsumableAbnormalitiesAsync()
    {
        if (!InHuntingZone)
            return;

        ConsumableAbnormalities consumableAbnormalities = await Memory.DerefPtrAsync<ConsumableAbnormalities>(
            address: _address,
            offsets: AddressMap.GetOffsets("Player::Abnormalities::Consumables")
        );


        foreach (AbnormalityDefinition definition in _consumableDefinitions.Value)
        {
            int maxTimerOffset = definition.Offset;
            int timerOffset = maxTimerOffset + sizeof(float);

            var data = new UpdateAbnormalityData
            {
                MaxTimer = BitConverter.ToSingle(consumableAbnormalities.Raw, maxTimerOffset),
                Timer = BitConverter.ToSingle(consumableAbnormalities.Raw, timerOffset)
            };

            HandleAbnormality(
                abnormalities: _abnormalities,
                schema: definition,
                timer: data.Timer,
                newData: data,
                activator: () => new MHWildsAbnormality(definition, AbnormalityType.Consumable)
            );
        }
    }

    [ScannableMethod]
    internal async Task GetSongAbnormalitiesAsync()
    {
        if (!InHuntingZone)
            return;

        SongAbnormalities songsAbnormalities = await Memory.DerefPtrAsync<SongAbnormalities>(
            address: _address,
            offsets: AddressMap.GetOffsets("Player::Abnormalities::Songs")
        );

        AbnormalityDefinition[] songDefinitions = _songsDefinitions.Value;

        float[] songTimers = await Memory.ReadArraySafeAsync<float>(
            address: songsAbnormalities.TimersPointer,
            count: songDefinitions.Length
        );
        float[] songMaxTimers = await Memory.ReadArraySafeAsync<float>(
            address: songsAbnormalities.MaxTimersPointer,
            count: songDefinitions.Length
        );

        for (int i = 0; i < songDefinitions.Length; i++)
        {
            if (i >= songTimers.Length || i >= songMaxTimers.Length)
                break;

            AbnormalityDefinition definition = songDefinitions[i];
            var data = new UpdateAbnormalityData
            {
                Timer = songTimers[i],
                MaxTimer = songMaxTimers[i]
            };

            HandleAbnormality(
                abnormalities: _abnormalities,
                schema: songDefinitions[i],
                timer: data.Timer,
                newData: data,
                activator: () => new MHWildsAbnormality(definition, AbnormalityType.Song)
            );
        }
    }

    [ScannableMethod]
    internal async Task GetDebuffsAbnormalitiesAsync()
    {
        if (!InHuntingZone)
            return;

        nint debuffsComponent = await Memory.ReadPtrAsync(
            address: _address,
            offsets: AddressMap.GetOffsets("Player::Abnormalities::Debuffs")
        );

        AbnormalityDefinition[] definitions = _debuffDefinitions.Value;

        nint[] debuffPointers = await Memory.ReadAsync<nint>(
            address: debuffsComponent + 0x10,
            count: _debuffIndexMax.Value + 1
        );

        foreach (AbnormalityDefinition definition in definitions)
        {
            if (definition.Index >= debuffPointers.Length)
                continue;

            nint debuffPointer = debuffPointers[definition.Index];

            UpdateAbnormalityData data;

            if (definition.IsBuildup)
            {
                DebuffAbnormality abnormality = await Memory.ReadAsync<DebuffAbnormality>(
                    address: debuffPointer
                );

                data = new UpdateAbnormalityData
                {
                    Timer = abnormality.BuildUp,
                    MaxTimer = definition.MaxBuildup
                };
            }
            else if (!definition.HasMaxTimer)
            {
                data = new UpdateAbnormalityData
                {
                    ShouldInferMaxTimer = true,
                    Timer = await Memory.ReadAsync<float>(
                        address: debuffPointer + definition.Offset
                    )
                };
            }
            else
            {
                float[] timers = await Memory.ReadAsync<float>(
                    address: debuffPointer + definition.Offset,
                    count: 2
                );
                byte isActive = await Memory.ReadAsync<byte>(
                    address: debuffPointer + 0x2F
                );

                data = new UpdateAbnormalityData
                {
                    Timer = isActive == 1 ? timers[0] : 0,
                    MaxTimer = timers[1]
                };
            }

            HandleAbnormality(
                abnormalities: _abnormalities,
                schema: definition,
                timer: data.Timer,
                newData: data,
                activator: () => new MHWildsAbnormality(definition, AbnormalityType.Debuff)
            );
        }
    }
}