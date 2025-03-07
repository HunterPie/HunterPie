using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Game;
using HunterPie.Core.Scan.Service;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Game;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds;

public class MHWildsContext : Context
{
    internal MHWildsContext(
        IGameProcess process,
        IScanService scanService) : base(new MHWildsGame(process, scanService), process)
    {
    }
}