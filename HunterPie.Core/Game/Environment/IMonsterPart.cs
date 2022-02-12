using System;

namespace HunterPie.Core.Game.Environment
{
    public interface IMonsterPart
    {

        public string Id { get; }
        public float Health { get; }
        public float MaxHealth { get; }
        public float Flinch { get; }
        public float MaxFlinch { get; }
        public float Tenderize { get; }
        public float MaxTenderize { get; }

        public event EventHandler<IMonsterPart> OnHealthUpdate;
        public event EventHandler<IMonsterPart> OnTenderizeUpdate;
        public event EventHandler<IMonsterPart> OnFlinchUpdate;
        public event EventHandler<IMonsterPart> OnBreakCountUpdate;

    }
}
