using HunterPie.Core.Game.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Core.Game.Rise.Entities
{
    public class MHRPartyMember : IPartyMember
    {
        private string _name;
        private int _damage;

        public string Name => throw new NotImplementedException();
        public int Damage => throw new NotImplementedException();
        public bool IsPlayer => throw new NotImplementedException();
    }
}
