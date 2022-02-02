using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Core.Game.Environment
{
    public interface IMonsterAilment
    {
        public string Id { get; }
        public int Counter { get; }
        public float Timer { get; }
        public float MaxTimer { get; }
        public float BuildUp { get; }
        public float MaxBuildUp { get; }

        public event EventHandler<IMonsterAilment> OnTimerUpdate;
        public event EventHandler<IMonsterAilment> OnBuildUpUpdate;
        public event EventHandler<IMonsterAilment> OnCounterUpdate;
    }
}
