using HunterPie.Core.Domain.Process;
using HunterPie.Core.Game.World.Data;

namespace HunterPie.Core.Game.World;

public sealed class MHWContext : Context
{
    public static MHWStrings Strings { get; private set;  }

    internal MHWContext(IProcessManager process)
    {
        Strings = new MHWStrings(process);
        Game = new MHWGame(process);
        Process = process;
    }

    public override void Dispose() { }
}
