using HunterPie.Playground.Dogma.Configuration;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.ViewModels;

namespace HunterPie.Playground.Dogma.ViewModels;

internal class DragonsDogmaMonstersViewModel(
    DragonsDogmaHealthPluginConfiguration config
) : WidgetViewModel(config, "Dragons Dogma Monster Widget", WidgetType.ClickThrough)
{
    public DragonsDogmaMonsterViewModel? Target { get; set => SetValue(ref field, value); }
}
