using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Rise.Definitions;
using HunterPie.Core.Native.IPC.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.Core.Game.Rise.Entities.Party
{
    public class MHRParty : IParty, IEventDispatcher
    {
        private readonly Dictionary<int, MHRPartyMember> _partyMembers = new();
        
        public const int MAX_PARTY_SIZE = 4;
        public int Size { get; internal set; }
        public int MaxSize => MAX_PARTY_SIZE;

        public List<IPartyMember> Members => _partyMembers.Values.ToList<IPartyMember>();

        public event EventHandler<IPartyMember> OnMemberJoin;
        public event EventHandler<IPartyMember> OnMemberLeave;
        
        public void Update(int memberIndex, MHRPartyMemberData data)
        {
            if (!_partyMembers.ContainsKey(memberIndex))
                Add(memberIndex, data);

            IUpdatable<MHRPartyMemberData> updatable = _partyMembers[memberIndex];
            updatable.Update(data);
        }

        public void Update(int memberIndex, EntityDamageData data)
        {
            if (!_partyMembers.ContainsKey(memberIndex))
                return;

            IUpdatable<EntityDamageData> updatable = _partyMembers[memberIndex];
            updatable.Update(data);
        }

        public void Add(int memberIndex, MHRPartyMemberData data)
        {
            _partyMembers.Add(memberIndex, new());

            IUpdatable<MHRPartyMemberData> updatable = _partyMembers[memberIndex];
            updatable.Update(data);

            this.Dispatch(OnMemberJoin, _partyMembers[memberIndex]);
        }

        public void Remove(int memberIndex)
        {
            if (!_partyMembers.ContainsKey(memberIndex))
                return;

            IPartyMember member = _partyMembers[memberIndex];
            _partyMembers.Remove(memberIndex);
            this.Dispatch(OnMemberLeave, member);
        }
    }
}
