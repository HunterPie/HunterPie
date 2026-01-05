using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Native.IPC.Models.Common;
using HunterPie.Integrations.Datasources.Common.Entity.Party;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Party;

public sealed class MHRPartyMember : CommonPartyMember, IUpdatable<MHRPartyMemberData>, IUpdatable<EntityDamageData>
{
    private int _damage;
    private Weapon _weapon;

    public override string Name { get; protected set; } = string.Empty;

    public override int Damage
    {
        get => _damage;
        protected set
        {
            if (value != _damage)
            {
                _damage = value;
                this.Dispatch(_onDamageDealt, this);
            }
        }
    }

    public override Weapon Weapon
    {
        get => _weapon;
        protected set
        {
            if (value != _weapon)
            {
                _weapon = value;
                this.Dispatch(_onWeaponChange, this);
            }
        }
    }

    public override int Slot { get; protected set; }

    public override bool IsMyself { get; protected set; } = true;
    public override MemberType Type { get; protected set; }

    public int HighRank { get; private set; }
    public override int MasterRank { get; protected set; }

    private readonly MHRPlayerStatus? _status;
    public override IPlayerStatus? Status => _status;

    public MHRPartyMember(MemberType type, bool isLocalPlayer)
    {
        Type = type;

        if (isLocalPlayer)
            _status = new MHRPlayerStatus();
    }

    public void Update(MHRPartyMemberData data)
    {
        Name = data.Name;
        Weapon = data.WeaponId;
        IsMyself = data.IsMyself;
        Slot = data.Slot;
        Type = data.MemberType;
        HighRank = data.HighRank;
        MasterRank = data.MasterRank;

        if (data.Status is { } status)
            _status?.Update(status);
    }

    public void Update(EntityDamageData data)
    {
        float totalDamage = data.RawDamage + data.ElementalDamage;
        Damage = (int)totalDamage;
    }

    public string GetHash() => $"{Name}:{HighRank}:{MasterRank}";

    public override void Dispose()
    {
        base.Dispose();
        _status?.Dispose();
    }
}