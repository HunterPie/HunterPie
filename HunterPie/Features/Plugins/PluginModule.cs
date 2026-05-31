using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Plugins.Services;

namespace HunterPie.Features.Plugins;

internal class PluginModule : IDependencyModule, IScopedModule
{
    void IDependencyModule.Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<PluginProvider>();
    }

    void IScopedModule.Register(IScopedDependencyRegistry registry)
    {
        registry
            .WithSingle<PluginLoader>();
    }
}
