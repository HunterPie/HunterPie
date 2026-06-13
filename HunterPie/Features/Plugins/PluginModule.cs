using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Plugins.Repository;
using HunterPie.Features.Plugins.Services;

namespace HunterPie.Features.Plugins;

internal class PluginModule : IDependencyModule, IScopedModule
{
    void IDependencyModule.Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<LocalPluginRepository>();
    }

    void IScopedModule.Register(IScopedDependencyRegistry registry)
    {
        registry
            .WithSingle(static r => new PluginLoader(
                repository: r.Get<IPluginRepository>(),
                registry: r
            ));
    }
}