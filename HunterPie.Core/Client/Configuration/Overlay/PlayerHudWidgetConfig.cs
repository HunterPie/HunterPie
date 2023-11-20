using HunterPie.Core.Architecture;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Annotations;
using HunterPie.Core.Settings.Common;
using HunterPie.Core.Settings.Types;

namespace HunterPie.Core.Client.Configuration.Overlay;

[Configuration("PLAYER_HUD_WIDGET_STRING", "ICON_HELM", availableGames: GameProcess.MonsterHunterRise | GameProcess.MonsterHunterWorld)]
public class PlayerHudWidgetConfig : IWidgetSettings, ISettings
{
    [ConfigurationProperty("INITIALIZE_WIDGET_STRING", requiresRestart: true, group: CommonConfigurationGroups.WIDGET)]
    public Observable<bool> Initialize { get; set; } = true;

    [ConfigurationProperty("ENABLE_WIDGET_STRING", group: CommonConfigurationGroups.WIDGET)]
    public Observable<bool> Enabled { get; set; } = true;

    [ConfigurationProperty("HIDE_WHEN_UI_VISIBLE_STRING", group: CommonConfigurationGroups.WIDGET)]
    public Observable<bool> HideWhenUiOpen { get; set; } = false;

    [ConfigurationProperty("WIDGET_OPACITY", group: CommonConfigurationGroups.WIDGET)]
    public Range Opacity { get; set; } = new(1, 1, 0.1, 0.1);

    [ConfigurationProperty("WIDGET_SCALE", group: CommonConfigurationGroups.WIDGET)]
    public Range Scale { get; set; } = new(1, 2, 0.1, 0.1);

    [ConfigurationProperty("ENABLE_STREAMER_MODE", group: CommonConfigurationGroups.WIDGET)]
    public Observable<bool> StreamerMode { get; set; } = false;

    [ConfigurationProperty("WIDGET_POSITION", group: CommonConfigurationGroups.WIDGET)]
    public Position Position { get; set; } = new(50, 0);
}
