using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Enums;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

public struct MHRPartyMemberData
{
    public int Index;
    public string Name;
    public Weapon WeaponId;
    public int HighRank;
    public int MasterRank;
    public bool IsMyself;
    public MemberType MemberType;

    public MHRPartyMemberData ToPetData()
    {
        MHRPartyMemberData copy = this.Copy();
        copy.Index = copy.Index.ToPetId();
        copy.MemberType = MemberType.Pet;
        return copy;
    }

    public string GetHash() => $"{Name}:{HighRank}:{MasterRank}";
}
