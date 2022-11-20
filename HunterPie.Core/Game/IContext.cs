using HunterPie.Core.Domain.Process;
using HunterPie.Core.Game.Entity.Game;

namespace HunterPie.Core.Game;
public interface IContext
{
    public IGame Game { get; }
    public IProcessManager Process { get; }
}
