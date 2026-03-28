using HunterPie.Core.Plugins.Configuration;
using HunterPie.Core.Plugins.DI;
using HunterPie.DI;
using HunterPie.Playground.Plugin.Configuration;

namespace HunterPie.Playground.Plugin;

internal class ExampleModule : IPluginModule
{
    public PluginConfiguration Configuration { get; } = new ExamplePluginConfigurationV1();

    public void Register(IDependencyRegistry registry)
    {
        registry.WithFactory<ExamplePlugin>();
    }
}
