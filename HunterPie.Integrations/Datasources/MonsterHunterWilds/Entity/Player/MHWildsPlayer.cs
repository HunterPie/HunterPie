using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Party;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Entity.Player.Vitals;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Scan.Service;
using HunterPie.Integrations.Datasources.Common.Entity.Player;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Player;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Player.Weapons;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Utils;
using WeaponType = HunterPie.Core.Game.Enums.Weapon;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Player;

public sealed class MHWildsPlayer : CommonPlayer
{
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
    public override IReadOnlyCollection<IAbnormality> Abnormalities { get; } = Array.Empty<IAbnormality>();
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
        _address = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("Game::PlayerManager"),
            offsets: AddressMap.GetOffsets("Player::Local")
        );

        MHWildsPlayerContext context = await Memory.DerefPtrAsync<MHWildsPlayerContext>(
            address: _address,
            offsets: AddressMap.GetOffsets("Player::Context")
        );

        Name = await Memory.ReadStringSafeAsync(context.NamePointer, size: 64);
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
}