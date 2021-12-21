using HunterPie.Core.Domain.Process;
using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Environment;
using HunterPie.Core.Game.World.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Core.Game.World
{
    public class MHWGame : IGame
    {
        private readonly MHWPlayer _player;
        private readonly List<IMonster> _monsters;
        private readonly IProcessManager _process;

        public IPlayer Player => _player;
        public List<IMonster> Monsters => _monsters;

        public MHWGame(IProcessManager process)
        {
            _process = process;
            _player = new(process);
        }
    }
}
