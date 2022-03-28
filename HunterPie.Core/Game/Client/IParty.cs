using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Core.Game.Client
{
    public interface IParty
    {
        public int Size { get; }
        public int MaxSize { get; }
        public List<IPartyMember> Members { get; }
    }
}
