using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.World.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.Core.Game.World.Entities.Party
{
    public class MHWParty : IParty, IEventDispatcher
    {
        public const int MAXIMUM_SIZE = 4;

        private readonly Dictionary<long, MHWPartyMember> _partyMembers = new(MAXIMUM_SIZE);

        public int Size => _partyMembers.Count;

        public int MaxSize => MAXIMUM_SIZE;

        public List<IPartyMember> Members => _partyMembers.Values.ToList<IPartyMember>();

        public event EventHandler<IPartyMember> OnMemberJoin;
        public event EventHandler<IPartyMember> OnMemberLeave;

        public void Update(long memberAddress, MHWPartyMemberData data)
        {
            if (!_partyMembers.ContainsKey(memberAddress))
                Add(memberAddress, data);

            IUpdatable<MHWPartyMemberData> updatable = _partyMembers[memberAddress];
            updatable.Update(data);
        }

        public void Add(long memberAddress, MHWPartyMemberData data)
        {
            _partyMembers.Add(memberAddress, new());
            
            IUpdatable<MHWPartyMemberData> updatable = _partyMembers[memberAddress];
            updatable.Update(data);

            this.Dispatch(OnMemberJoin, _partyMembers[memberAddress]);
        }

        public void Remove(long memberAddress)
        {
            if (!_partyMembers.ContainsKey(memberAddress))
                return;

            IPartyMember member = _partyMembers[memberAddress];
            _partyMembers.Remove(memberAddress);
            this.Dispatch(OnMemberLeave, member);
        }
    }
}
