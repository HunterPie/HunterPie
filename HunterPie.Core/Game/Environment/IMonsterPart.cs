using System;

namespace HunterPie.Core.Game.Environment
{
    public interface IMonsterPart
    {

        public string Id { get; }
        public float Health { get; }
        public float MaxHealth { get; }

        public event EventHandler<IMonsterPart> OnHealthUpdate;
        public event EventHandler<IMonsterPart> OnBreakCountUpdate;

    }
}
