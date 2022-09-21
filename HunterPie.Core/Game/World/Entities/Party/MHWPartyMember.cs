using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.World.Definitions;
using HunterPie.Core.Native.IPC.Models.Common;
using System;

namespace HunterPie.Core.Game.World.Entities.Party;

public class MHWPartyMember : IPartyMember, IEventDispatcher, IUpdatable<MHWPartyMemberData>, IUpdatable<EntityDamageData>
{
    private int _damage;
    private Weapon _weapon;
    private bool _anyNonTrivialStatisticalDamage;

    public string Name { get; private set; }

    public int Damage
    {
        get => _damage;
        private set
        {
            if (value != _damage)
            {
                _damage = value;
                this.Dispatch(OnDamageDealt, this);
            }
        }
    }

    public Weapon Weapon
    {
        get => _weapon;
        private set
        {
            if (value != _weapon)
            {
                _weapon = value;
                this.Dispatch(OnWeaponChange, this);
            }
        }
    }

    public int Slot { get; private set; }

    public bool IsMyself { get; private set; }

    public MemberType Type => MemberType.Player;

    public int MasterRank { get; private set; }

    public event EventHandler<IPartyMember> OnDamageDealt;
    public event EventHandler<IPartyMember> OnWeaponChange;

    /// <inheritdoc />
    void IUpdatable<MHWPartyMemberData>.Update(MHWPartyMemberData data)
    {
        Name = data.Name;
        if (data.Damage != 0)
        {
            _anyNonTrivialStatisticalDamage = true;
            Damage = data.Damage;
        }
        Weapon = data.Weapon;
        Slot = data.Slot;
        IsMyself = data.IsMyself;
        MasterRank = data.MasterRank;
    }

    /// <inheritdoc />
    void IUpdatable<EntityDamageData>.Update(EntityDamageData data)
    {
        // If there is only trivial (zero) damage data from player statistics data,
        // we will use EntityDamageData from HunterPie.Native as a fallback.
        if (!_anyNonTrivialStatisticalDamage)
        {
            Damage = (int)(data.RawDamage + data.ElementalDamage);
        }
    }
}
