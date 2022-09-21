using HunterPie.Core.Domain.Process;
using HunterPie.Core.Game.Rise.Data;

namespace HunterPie.Core.Game.Demos.Sunbreak;

public class MHRSunbreakDemoContext : Context
{
    public static MHRStrings Strings { get; private set; }

    internal MHRSunbreakDemoContext(IProcessManager process)
    {
        Strings = new MHRStrings(process);
        Game = new MHRSunbreakDemoGame(process);
        Process = process;
    }

    public override void Dispose()
    {
    }
}
