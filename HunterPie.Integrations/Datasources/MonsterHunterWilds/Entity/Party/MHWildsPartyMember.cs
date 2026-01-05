using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.Common.Entity.Party;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Party.Data;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Player;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Party;

public sealed class MHWildsPartyMember : CommonPartyMember, IUpdatable<UpdatePartyMember>
{
    public override string Name { get; protected set; } = string.Empty;

    public override int MasterRank { get; protected set; }

    private int _damage;
    public override int Damage
    {
        get => _damage;
        protected set
        {
            if (value == _damage)
                return;

            _damage = value;
            this.Dispatch(
                toDispatch: _onDamageDealt,
                data: this
            );
        }
    }

    private Weapon _weapon;
    public override Weapon Weapon
    {
        get => _weapon;
        protected set
        {
            if (value == _weapon)
                return;

            _weapon = value;
            this.Dispatch(
                toDispatch: _onWeaponChange,
                data: this
            );
        }
    }

    public override int Slot { get; protected set; }

    public override bool IsMyself { get; protected set; }

    public override MemberType Type { get; protected set; }

    private readonly MHWildsPlayerStatus _status = new();
    public override IPlayerStatus Status => _status;

    public void Update(UpdatePartyMember data)
    {
        Type = data.IsNpc
            ? MemberType.Companion
            : MemberType.Player;
        Name = data.Name;
        Weapon = data.Weapon;
        Damage = (int)data.Damage;
        Slot = data.Index;
        IsMyself = data.IsMyself;
        MasterRank = data.HunterRank;

        if (data.IsNpc)
            return;

        _status.Update(data.Status);
    }

    public override void Dispose()
    {
        base.Dispose();
        _status.Dispose();
    }
}