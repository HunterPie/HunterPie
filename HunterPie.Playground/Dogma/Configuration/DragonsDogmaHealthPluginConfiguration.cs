using HunterPie.Core.Plugins.Configuration;

namespace HunterPie.Playground.Dogma.Configuration;

internal class DragonsDogmaHealthPluginConfiguration : PluginConfiguration
{
    public override int Version => 1;

    public MonsterWidgetConfiguration MonsterWidget { get; set; } = new MonsterWidgetConfiguration();
}
