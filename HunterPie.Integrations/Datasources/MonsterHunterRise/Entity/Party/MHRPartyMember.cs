using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Party;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Native.IPC.Models.Common;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Party;

public class MHRPartyMember : IPartyMember, IEventDispatcher, IUpdatable<MHRPartyMemberData>, IUpdatable<EntityDamageData>
{
    private int _damage;
    private Weapon _weapon;

    public string Name { get; private set; }

    public int Damage
    {
        get => _damage;
        private set
        {
            if (value != _damage)
            {
                _damage = value;
                this.Dispatch(_onDamageDealt, this);
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
                this.Dispatch(_onWeaponChange, this);
            }
        }
    }

    public int Slot { get; private set; }

    public bool IsMyself { get; private set; } = true;
    public MemberType Type { get; private set; }

    public int MasterRank { get; private set; }

    private readonly SmartEvent<IPartyMember> _onDamageDealt = new();
    public event EventHandler<IPartyMember> OnDamageDealt
    {
        add => _onDamageDealt.Hook(value);
        remove => _onDamageDealt.Unhook(value);
    }

    private readonly SmartEvent<IPartyMember> _onWeaponChange = new();
    public event EventHandler<IPartyMember> OnWeaponChange
    {
        add => _onWeaponChange.Hook(value);
        remove => _onWeaponChange.Unhook(value);
    }

    public MHRPartyMember() { }

    public MHRPartyMember(MemberType type)
    {
        Type = type;
    }

    void IUpdatable<MHRPartyMemberData>.Update(MHRPartyMemberData data)
    {
        Name = data.Name;
        Weapon = data.WeaponId;
        IsMyself = data.IsMyself;
        Slot = data.Index;
        Type = data.MemberType;
        MasterRank = data.MasterRank;
    }

    void IUpdatable<EntityDamageData>.Update(EntityDamageData data)
    {
        float totalDamage = data.RawDamage + data.ElementalDamage;
        Damage = (int)totalDamage;
    }
}
