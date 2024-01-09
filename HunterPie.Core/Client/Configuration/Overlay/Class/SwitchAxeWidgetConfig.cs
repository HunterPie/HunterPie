using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings.Annotations;

namespace HunterPie.Core.Client.Configuration.Overlay.Class;

[Configuration("SWITCH_AXE_WIDGET_STRING", "ICON_SWITCHAXE", availableGames: GameProcess.MonsterHunterRise | GameProcess.MonsterHunterWorld)]
public class SwitchAxeWidgetConfig : ClassWidgetConfig
{

}