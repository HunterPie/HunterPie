using HunterPie.Core.Domain.Process;
using HunterPie.Core.Game;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Services;
using HunterPie.Integrations.Datasources.MonsterHunterSunbreakDemo.Entity.Game;

namespace HunterPie.Integrations.Datasources.MonsterHunterSunbreakDemo;

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
        Game.Dispose();
    }
}
