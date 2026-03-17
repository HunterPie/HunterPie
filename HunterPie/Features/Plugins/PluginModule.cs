using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Plugins.Services;

namespace HunterPie.Features.Plugins;

internal class PluginModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry.WithSingle<PluginLoader>();
    }
}
