using HunterPie.Core.Domain.Process;
using HunterPie.Core.Game;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Game;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Services;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise;

public sealed class MHRContext : Context
{
    public static MHRStrings Strings { get; private set; }

    internal MHRContext(IProcessManager process)
    {
        Strings = new MHRStrings(process);
        Game = new MHRGame(process);
        Process = process;
    }
}