using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Annotations;
using HunterPie.Core.Settings.Common;
using HunterPie.Core.Settings.Types;
using System;
using Range = HunterPie.Core.Settings.Types.Range;

namespace HunterPie.Core.Client.Configuration.Overlay;

[Configuration(name: "METER_WIDGET_STRING",
    icon: "ICON_METER",
    group: CommonConfigurationGroups.OVERLAY,
    availableGames: GameProcessType.MonsterHunterWorld | GameProcessType.MonsterHunterRise | GameProcessType.MonsterHunterWilds)]
public class DamageMeterWidgetConfig : IWidgetSettings, ISettings
{
    #region Widget
    [ConfigurationProperty("INITIALIZE_WIDGET_STRING", requiresRestart: true, group: CommonConfigurationGroups.WIDGET)]
    public Observable<bool> Initialize { get; set; } = true;

    [ConfigurationProperty("ENABLE_WIDGET_STRING", group: CommonConfigurationGroups.WIDGET)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<bool> Enabled { get; set; } = true;

    [ConfigurationProperty("HIDE_WHEN_UI_VISIBLE_STRING", group: CommonConfigurationGroups.WIDGET)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<bool> HideWhenUiOpen { get; set; } = false;

    [ConfigurationProperty("WIDGET_POSITION", group: CommonConfigurationGroups.WIDGET)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Position Position { get; set; } = new(0, 400);

    [ConfigurationProperty("WIDGET_OPACITY", group: CommonConfigurationGroups.WIDGET)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Range Opacity { get; set; } = new(1, 1, 0, 0.1);

    [ConfigurationProperty("WIDGET_SCALE", group: CommonConfigurationGroups.WIDGET)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Range Scale { get; set; } = new(1, 2, 0, 0.1);
    #endregion

    #region Customizations
    [ConfigurationProperty("DAMAGE_METER_ENABLE_SHOULD_HIGHLIGHT_MYSELF", group: CommonConfigurationGroups.CUSTOMIZATIONS)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<bool> ShouldHighlightMyself { get; set; } = false;

    [ConfigurationProperty("DAMAGE_METER_ENABLE_SHOULD_BLUR_NAMES", group: CommonConfigurationGroups.CUSTOMIZATIONS)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<bool> ShouldBlurNames { get; set; } = false;

    [ConfigurationProperty("DAMAGE_METER_ENABLE_OTOMOS", group: CommonConfigurationGroups.CUSTOMIZATIONS, availableGames: GameProcessType.MonsterHunterRise)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<bool> ShouldShowOtomos { get; set; } = true;

    [ConfigurationProperty("DAMAGE_METER_SHOW_ONLY_SELF_STRING", group: CommonConfigurationGroups.CUSTOMIZATIONS)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<bool> ShowOnlySelf { get; set; } = false;

    [ConfigurationProperty("DAMAGE_METER_DPS_CALCULATION_STRATEGY_STRING", group: CommonConfigurationGroups.CUSTOMIZATIONS)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<DPSCalculationStrategy> DpsCalculationStrategy { get; set; } = DPSCalculationStrategy.RelativeToJoin;

    [ConfigurationProperty("DAMAGE_METER_SHOULD_SHOW_DPS", group: CommonConfigurationGroups.CUSTOMIZATIONS)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<bool> ShouldShowDPS { get; set; } = true;

    [ConfigurationProperty("DAMAGE_METER_SHOULD_SHOW_DAMAGE", group: CommonConfigurationGroups.CUSTOMIZATIONS)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<bool> ShouldShowDamage { get; set; } = true;

    [ConfigurationProperty("DAMAGE_METER_SHOULD_SHOW_STATUS", group: CommonConfigurationGroups.GEAR_STATUS, availableGames: GameProcessType.MonsterHunterWilds | GameProcessType.MonsterHunterRise)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<bool> IsStatusEnabled { get; set; } = true;

    [ConfigurationProperty("DAMAGE_METER_SHOULD_SHOW_AFFINITY", group: CommonConfigurationGroups.GEAR_STATUS, availableGames: GameProcessType.MonsterHunterWilds | GameProcessType.MonsterHunterRise)]
    [ConfigurationConditional(name: nameof(IsStatusEnabled), withValue: true)]
    public Observable<bool> IsAffinityEnabled { get; set; } = true;

    [ConfigurationProperty("DAMAGE_METER_SHOULD_SHOW_RAW_DAMAGE", group: CommonConfigurationGroups.GEAR_STATUS, availableGames: GameProcessType.MonsterHunterWilds | GameProcessType.MonsterHunterRise)]
    [ConfigurationConditional(name: nameof(IsStatusEnabled), withValue: true)]
    public Observable<bool> IsRawDamageEnabled { get; set; } = true;

    [ConfigurationProperty("DAMAGE_METER_SHOULD_SHOW_ELEMENTAL_DAMAGE", group: CommonConfigurationGroups.GEAR_STATUS, availableGames: GameProcessType.MonsterHunterWilds | GameProcessType.MonsterHunterRise)]
    [ConfigurationConditional(name: nameof(IsStatusEnabled), withValue: true)]
    public Observable<bool> IsElementalDamageEnabled { get; set; } = true;
    #endregion

    #region Plotting
    [ConfigurationProperty("DAMAGE_METER_ENABLE_DPS_PLOT", group: CommonConfigurationGroups.DAMAGE_PLOT)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<bool> ShouldShowPlots { get; set; } = true;

    [Obsolete]
    public Observable<bool> IsPlotSlidingWindowEnabled { get; set; } = true;

    [ConfigurationProperty("DAMAGE_METER_DAMAGE_PLOT_STRATEGY_STRING", group: CommonConfigurationGroups.DAMAGE_PLOT)]
    [ConfigurationConditional(name: nameof(ShouldShowPlots), withValue: true)]
    public Observable<DamagePlotStrategy> DamagePlotStrategy { get; set; } = Enums.DamagePlotStrategy.DamagePerSecond;

    [ConfigurationProperty("DAMAGE_METER_SAMPLING_RATE", group: CommonConfigurationGroups.DAMAGE_PLOT)]
    [ConfigurationConditional(name: nameof(DamagePlotStrategy), withValue: Enums.DamagePlotStrategy.MovingAverageDamagePerSecond)]
    public Range PlotSamplingInSeconds { get; set; } = new Range(10.0, 60.0, 1.0, 1.0);

    [ConfigurationProperty("DAMAGE_METER_SLIDING_WINDOW_DISCARD_OLD_PLOTS", group: CommonConfigurationGroups.DAMAGE_PLOT)]
    [ConfigurationConditional(name: nameof(ShouldShowPlots), withValue: true)]
    public Observable<bool> IsOldPlotDiscardingEnabled { get; set; } = false;

    [ConfigurationProperty("DAMAGE_METER_SLIDING_WINDOW_SECONDS", group: CommonConfigurationGroups.DAMAGE_PLOT)]
    [ConfigurationConditional(name: nameof(IsOldPlotDiscardingEnabled), withValue: true)]
    public Range PlotSlidingWindowInSeconds { get; set; } = new Range(10.0, 120, 5.0, 1.0);

    [ConfigurationProperty("DAMAGE_METER_PLOT_LINE_SMOOTHING_STRING", group: CommonConfigurationGroups.DAMAGE_PLOT, AvailableGames = GameProcessType.MonsterHunterRise)]
    [ConfigurationConditional(name: nameof(ShouldShowPlots), withValue: true)]
    public Range PlotLineSmoothing { get; set; } = new Range(0.0, 1.0, 0.0, 0.01);

    [ConfigurationProperty("DAMAGE_METER_PLOT_LINE_THICKNESS_STRING", group: CommonConfigurationGroups.DAMAGE_PLOT, AvailableGames = GameProcessType.MonsterHunterRise)]
    [ConfigurationConditional(name: nameof(ShouldShowPlots), withValue: true)]
    public Range PlotLineThickness { get; set; } = new Range(1.0, 3.0, 0.2, 0.01);
    #endregion

    #region Colors
    [ConfigurationProperty("DAMAGE_METER_SELF_COLOR_STRING", group: CommonConfigurationGroups.COLORS)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Color PlayerSelf { get; set; } = "#FFA74FFF";

    [ConfigurationProperty("DAMAGE_METER_PLAYER_1_COLOR_STRING", group: CommonConfigurationGroups.COLORS)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    [ConfigurationConditional(name: nameof(ShowOnlySelf), withValue: false)]
    public Color PlayerFirst { get; set; } = "#FFF24891";

    [ConfigurationProperty("DAMAGE_METER_PLAYER_2_COLOR_STRING", group: CommonConfigurationGroups.COLORS)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    [ConfigurationConditional(name: nameof(ShowOnlySelf), withValue: false)]
    public Color PlayerSecond { get; set; } = "#FF50C5B7";

    [ConfigurationProperty("DAMAGE_METER_PLAYER_3_COLOR_STRING", group: CommonConfigurationGroups.COLORS)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    [ConfigurationConditional(name: nameof(ShowOnlySelf), withValue: false)]
    public Color PlayerThird { get; set; } = "#FF49CFF5";

    [ConfigurationProperty("DAMAGE_METER_PLAYER_4_COLOR_STRING", group: CommonConfigurationGroups.COLORS)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    [ConfigurationConditional(name: nameof(ShowOnlySelf), withValue: false)]
    public Color PlayerFourth { get; set; } = "#FFFF8040";

    [ConfigurationProperty("DAMAGE_METER_NPC_COLOR_STRING", availableGames: GameProcessType.MonsterHunterRise | GameProcessType.MonsterHunterWilds, group: CommonConfigurationGroups.COLORS)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    [ConfigurationConditional(name: nameof(ShowOnlySelf), withValue: false)]
    public Color NpcColor { get; set; } = "#FFEBD5FF";
    #endregion
}