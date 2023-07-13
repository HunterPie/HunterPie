using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings;

namespace HunterPie.Core.Client.Configuration.Overlay.Class;

[SettingsGroup("CHARGE_BLADE_WIDGET_STRING", "ICON_CHARGEBLADE", availableGames: GameProcess.MonsterHunterRise | GameProcess.MonsterHunterWorld)]
public sealed class ChargeBladeWidgetConfig : ClassWidgetConfig
{

}