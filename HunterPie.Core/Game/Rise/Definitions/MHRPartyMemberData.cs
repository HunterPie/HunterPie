using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Enums;

namespace HunterPie.Core.Game.Rise.Definitions
{
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
    }
}
