using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Environment;
using System;
using System.Collections.Generic;

namespace HunterPie.Core.Game
{
    public interface IGame
    {

        public IPlayer Player { get; }
        public List<IMonster> Monsters { get; }

        public event EventHandler<IMonster> OnMonsterSpawn;
        public event EventHandler<IMonster> OnMonsterDespawn;

    }
}
