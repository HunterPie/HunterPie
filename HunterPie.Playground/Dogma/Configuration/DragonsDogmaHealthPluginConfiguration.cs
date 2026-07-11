using HunterPie.Core.Plugins.Configuration;
using HunterPie.Core.Settings.Annotations;
using HunterPie.Core.Settings.Common;

namespace HunterPie.Playground.Dogma.Configuration;

[Configuration(
    name: "DRAGONS_DOGMA",
    icon: "Icons.Plugin",
    group: CommonConfigurationGroups.CLIENT
)]
internal class DragonsDogmaHealthPluginConfiguration : PluginConfiguration
{
    public override int Version => 1;

    public MonsterWidgetConfiguration MonsterWidget { get; set; } = new MonsterWidgetConfiguration();
}
