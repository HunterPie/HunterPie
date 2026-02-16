using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Process;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds;

internal class MHWildsGameModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<MHWildsProcessAttachStrategy>()
            .WithFactory<MHWildsMonsterTargetKeyManager>();
    }
}