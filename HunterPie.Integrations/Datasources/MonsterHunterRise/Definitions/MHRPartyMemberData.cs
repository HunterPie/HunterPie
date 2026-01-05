using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player.Data;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

public struct MHRPartyMemberData
{
    public required int Index;
    public required int Slot;
    public required string Name;
    public required Weapon WeaponId;
    public required int HighRank;
    public required int MasterRank;
    public required bool IsMyself;
    public required MemberType MemberType;
    public required UpdatePlayerStatus? Status;

    public MHRPartyMemberData ToPetData() => this with { Index = Index.ToPetId(), MemberType = MemberType.Pet };

    public string GetHash() => $"{Name}:{HighRank}:{MasterRank}";
}