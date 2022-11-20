using HunterPie.Core.Domain.Process;
using HunterPie.Core.Game;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Game;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Services;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld;

public sealed class MHWContext : Context
{
    public static MHWStrings Strings { get; private set; }

    internal MHWContext(IProcessManager process)
    {
        Strings = new MHWStrings(process);
        Game = new MHWGame(process);
        Process = process;
    }

    public override void Dispose()
    {
        Game.Dispose();
    }
}
