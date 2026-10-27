using HunterPie.Core.Architecture;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Annotations;
using HunterPie.Core.Settings.Common;
using HunterPie.Core.Settings.Types;

namespace HunterPie.Core.Client.Configuration.Overlay;

[Configuration(name: "CLOCK_WIDGET_STRING",
    icon: "ICON_STOPWATCH",
    group: CommonConfigurationGroups.OVERLAY,
    availableGames: GameProcessType.MonsterHunterRise | GameProcessType.MonsterHunterWorld | GameProcessType.MonsterHunterWilds)]
public class ClockWidgetConfig : IWidgetSettings, ISettings
{
    #region General Settings
    [ConfigurationProperty("INITIALIZE_WIDGET_STRING", requiresRestart: true, group: CommonConfigurationGroups.GENERAL)]
    public Observable<bool> Initialize { get; set; } = true;

    [ConfigurationProperty("ENABLE_WIDGET_STRING", group: CommonConfigurationGroups.GENERAL)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<bool> Enabled { get; set; } = true;

    [ConfigurationProperty("HIDE_WHEN_UI_VISIBLE_STRING", group: CommonConfigurationGroups.GENERAL)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<bool> HideWhenUiOpen { get; set; } = false;

    [ConfigurationProperty("WIDGET_OPACITY", group: CommonConfigurationGroups.GENERAL)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Range Opacity { get; set; } = new(1, 1, 0.1, 0.1);

    [ConfigurationProperty("WIDGET_SCALE", group: CommonConfigurationGroups.GENERAL)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Range Scale { get; set; } = new(1, 2, 0.1, 0.1);

    [ConfigurationProperty("WIDGET_POSITION", group: CommonConfigurationGroups.GENERAL)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Position Position { get; set; } = new(50, 0);
    #endregion

    [ConfigurationProperty("IS_MOON_PHASES_ENABLED", group: CommonConfigurationGroups.WIDGET, availableGames: GameProcessType.MonsterHunterWilds)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<bool> IsMoonPhaseEnabled { get; set; } = true;

    [ConfigurationProperty("WORLD_TIME_24H_FORMAT", group: CommonConfigurationGroups.WIDGET)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<bool> Use24HourFormat { get; set; } = false;
}