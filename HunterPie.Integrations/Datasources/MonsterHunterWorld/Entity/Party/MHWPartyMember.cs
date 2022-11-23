using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Party;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Native.IPC.Models.Common;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Party;

public class MHWPartyMember : IPartyMember, IEventDispatcher, IUpdatable<MHWPartyMemberData>, IUpdatable<EntityDamageData>
{
    private int _damage;
    private Weapon _weapon;
    private bool _anyNonTrivialStatisticalDamage;

    public string Name { get; private set; } = string.Empty;

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

    public bool IsMyself { get; private set; }

    public MemberType Type => MemberType.Player;

    public int MasterRank { get; private set; }

    internal void ResetDamage()
    {
        _damage = 0;
        _anyNonTrivialStatisticalDamage = false;
    }

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

    public void Update(MHWPartyMemberData data)
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

    public void Update(EntityDamageData data)
    {
        // If there is only trivial (zero) damage data from player statistics data,
        // we will use EntityDamageData from HunterPie.Native as a fallback.
        if (!_anyNonTrivialStatisticalDamage)
            Damage = (int)(data.RawDamage + data.ElementalDamage);
    }
}
