using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Integrations.Patreon.Navigation;

namespace HunterPie.Integrations.Patreon;

internal class PatreonModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry.WithSingle<PatreonNavigationHandler>();
    }
}