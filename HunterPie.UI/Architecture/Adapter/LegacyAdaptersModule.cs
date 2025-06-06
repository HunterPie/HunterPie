using HunterPie.DI;
using HunterPie.DI.Module;

namespace HunterPie.UI.Architecture.Adapter;

internal class LegacyAdaptersModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry.WithSingle<MonsterNameAdapter>();
    }
}