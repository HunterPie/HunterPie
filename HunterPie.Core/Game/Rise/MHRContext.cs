using HunterPie.Core.Domain.Process;
using HunterPie.Core.Game.Rise.Data;

namespace HunterPie.Core.Game.Rise;

public sealed class MHRContext : Context
{
    public static MHRStrings Strings { get; private set; }

    internal MHRContext(IProcessManager process)
    {
        Strings = new MHRStrings(process);
        Game = new MHRGame(process);
        Process = process;
    }

    public override void Dispose() => Game.Dispose();
}
