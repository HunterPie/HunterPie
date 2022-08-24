using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.World.Definitions;
using System;

namespace HunterPie.Core.Game.World.Entities.Party
{
    public class MHWPartyMember : IPartyMember, IEventDispatcher, IUpdatable<MHWPartyMemberData>
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

        void IUpdatable<MHWPartyMemberData>.Update(MHWPartyMemberData data)
        {
            Name = data.Name;
            Damage = data.Damage;
            Weapon = data.Weapon;
            Slot = data.Slot;
            IsMyself = data.IsMyself;
            MasterRank = data.MasterRank;
        }
    }
}
