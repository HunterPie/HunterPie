using HunterPie.Core.Domain.Process;
using HunterPie.Core.Game.Data;

namespace HunterPie.Core.Game
{
    public abstract class Context
    {
        public IGame Game { get; protected set; }
        public IGameStrings Strings { get; protected set; }
        public IProcessManager Process { get; protected set; }
    }
}
