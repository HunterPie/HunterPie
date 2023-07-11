using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings;

namespace HunterPie.Core.Client.Configuration.Overlay.Class;

[SettingsGroup("DUALBLADES_WIDGET_STRING", "ICON_DUALBLADES", availableGames: GameProcess.MonsterHunterRise | GameProcess.MonsterHunterWorld)]
public sealed class DualBladesWidgetConfig : ClassWidgetConfig
{

}