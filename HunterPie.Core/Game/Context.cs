using HunterPie.Core.Domain.Process;

namespace HunterPie.Core.Game
{
    public abstract class Context
    {
        public IGame Game { get; protected set; }
        public IProcessManager Process { get; protected set; }

        internal abstract void Scan();
    }
}
