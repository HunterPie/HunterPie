using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Core.Game.Rise.Entities
{
    public class MHRPartyMember : IPartyMember
    {
        public string Name => throw new NotImplementedException();

        public int Damage => throw new NotImplementedException();

        public Weapon Weapon => throw new NotImplementedException();

        public int Slot => throw new NotImplementedException();

        public event EventHandler<IPartyMember> OnDamageDealt;
        public event EventHandler<IPartyMember> OnWeaponChange;
    }
}
