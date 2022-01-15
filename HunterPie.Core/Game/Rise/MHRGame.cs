using HunterPie.Core.Domain.Process;
using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Environment;
using HunterPie.Core.Game.Rise.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Core.Game.Rise
{
    public class MHRGame : IGame
    {
        public IPlayer Player => throw new NotImplementedException();

        public List<IMonster> Monsters { get; } = new();

        public MHRGame(IProcessManager process)
        {
            Monsters.Add(new MHRMonster(process));
        }
    }
}
