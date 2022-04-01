using HunterPie.Core.Game.Client;
using System.Collections.Generic;

namespace HunterPie.Core.Game.Rise.Entities.Party
{
    public class MHRParty : IParty
    {
        public const int MAX_PARTY_SIZE = 4;
        public int Size { get; internal set; }
        public int MaxSize => MAX_PARTY_SIZE;

        public List<IPartyMember> Members { get; } = new(MAX_PARTY_SIZE);
    }
}
