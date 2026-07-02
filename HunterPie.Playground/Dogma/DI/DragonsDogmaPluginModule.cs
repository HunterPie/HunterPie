using HunterPie.Core.Plugins.Configuration;
using HunterPie.Core.Plugins.DI;
using HunterPie.DI;
using HunterPie.Playground.Dogma.Configuration;
using HunterPie.Playground.Dogma.Controller;
using HunterPie.Playground.Dogma.Plugin;
using HunterPie.Playground.Dogma.ViewModels;

namespace HunterPie.Playground.Dogma.DI;

internal class DragonsDogmaPluginModule : IPluginModule
{
    public PluginConfiguration Configuration { get; } = new DragonsDogmaHealthPluginConfiguration();

    public void Register(IScopedDependencyRegistry registry)
    {
        registry
            .WithFactory<DragonsDogmaHealthPlugin>()
            .WithFactory<DragonsDogmaMonsterViewModel>()
            .WithFactory<DragonsDogmaMonstersViewModel>()
            .WithSingle<DragonsDogmaMonsterWidgetController>();
    }
}
