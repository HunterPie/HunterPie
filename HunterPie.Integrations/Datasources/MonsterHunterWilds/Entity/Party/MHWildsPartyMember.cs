using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.Common.Entity.Party;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Party.Data;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Party;

public class MHWildsPartyMember : CommonPartyMember, IUpdatable<UpdatePartyMember>
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

    public override MemberType Type { get; protected set; } = MemberType.Player;

    public void Update(UpdatePartyMember data)
    {
        Name = data.Name;
        Weapon = data.Weapon;
        Damage = (int)data.Damage;
        Slot = data.Index;
        IsMyself = data.IsMyself;
    }
}