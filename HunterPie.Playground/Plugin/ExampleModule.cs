using HunterPie.Core.Plugins.DI;
using HunterPie.DI;

namespace HunterPie.Playground.Plugin;

internal class ExampleModule : IPluginModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry.WithFactory<ExamplePlugin>();
    }
}
