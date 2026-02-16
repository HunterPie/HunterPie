using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Adapters;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Annotations;
using HunterPie.Core.Settings.Common;
using HunterPie.Core.Settings.Types;

namespace HunterPie.Core.Client.Configuration.Overlay;

[Configuration(name: "MONSTER_WIDGET_STRING",
    icon: "ICON_SKULL",
    group: CommonConfigurationGroups.OVERLAY)]
public class MonsterWidgetConfig : IWidgetSettings, ISettings
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
    public Position Position { get; set; } = new(1200, 100);
    #endregion

    #region Widget Settings
    [ConfigurationProperty("MONSTER_WIDGET_COMPACT_MODE", group: CommonConfigurationGroups.WIDGET)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<bool> IsCompactModeEnabled { get; set; } = true;

    [ConfigurationProperty("MONSTER_WIDGET_COMPACT_MODE_COLUMN_LIMIT", group: CommonConfigurationGroups.WIDGET)]
    [ConfigurationConditional(name: nameof(IsCompactModeEnabled), withValue: true)]
    public Range CompactModeColumnLimit { get; set; } = new Range(5, 10, 1, 1);

    [ConfigurationProperty("ORIENTATION_STRING", group: CommonConfigurationGroups.WIDGET)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<Orientation> Orientation { get; set; } = Enums.Orientation.Vertical;

    [ConfigurationProperty("MONSTER_WIDGET_DYNAMIC_RESIZE_STRING", group: CommonConfigurationGroups.WIDGET)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<bool> DynamicResize { get; set; } = false;

    [ConfigurationProperty("MONSTER_WIDGET_MAX_WIDTH_STRING", group: CommonConfigurationGroups.WIDGET)]
    [ConfigurationConditional(name: nameof(DynamicResize), withValue: true)]
    public Range MaxWidth { get; set; } = new(600, 1000, 200, 1);

    [ConfigurationProperty("MONSTER_WIDGET_MIN_WIDTH_STRING", group: CommonConfigurationGroups.WIDGET)]
    [ConfigurationConditional(name: nameof(DynamicResize), withValue: true)]
    public Range MinWidth { get; set; } = new(400, 600, 200, 1);
    #endregion

    #region Customization settings
    [ConfigurationProperty("MONSTER_WIDGET_ENABLE_MONSTER_TYPE_STRING", group: CommonConfigurationGroups.CUSTOMIZATIONS)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<bool> IsTypeEnabled { get; set; } = true;

    [ConfigurationProperty("MONSTER_WIDGET_ENABLE_HEALTH_STRING", group: CommonConfigurationGroups.CUSTOMIZATIONS)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<bool> IsHealthBarEnabled { get; set; } = true;

    [ConfigurationProperty("MONSTER_WIDGET_ENABLE_HEALTH_TEXT_STRING", group: CommonConfigurationGroups.CUSTOMIZATIONS)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<bool> IsHealthTextEnabled { get; set; } = true;

    [ConfigurationProperty("MONSTER_WIDGET_ENABLE_STAMINA_STRING", group: CommonConfigurationGroups.CUSTOMIZATIONS)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<bool> IsStaminaBarEnabled { get; set; } = true;

    [ConfigurationProperty("MONSTER_WIDGET_ENABLE_STAMINA_TEXT_STRING", group: CommonConfigurationGroups.CUSTOMIZATIONS)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<bool> IsStaminaTextEnabled { get; set; } = true;

    [ConfigurationProperty("MONSTER_WIDGET_ENABLE_WEAKNESS_STRING", group: CommonConfigurationGroups.CUSTOMIZATIONS)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<bool> IsWeaknessEnabled { get; set; } = true;
    #endregion

    #region Target settings

    [ConfigurationProperty("MONSTER_WIDGET_ENABLE_TARGETING", group: CommonConfigurationGroups.MONSTER_TARGET)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<bool> IsTargetingEnabled { get; set; } = true;

    [ConfigurationProperty("MONSTER_WIDGET_TARGET_MODE_STRING",
        group: CommonConfigurationGroups.MONSTER_TARGET)]
    [GameConfigurationAdapter(typeof(TargetModeEnumAdapter))]
    [ConfigurationConditional(name: nameof(IsTargetingEnabled), withValue: true)]
    public Observable<TargetModeType> TargetMode { get; set; } = TargetModeType.LockOn;

    [ConfigurationProperty("MONSTER_WIDGET_SHOW_ONLY_TARGET_STRING", group: CommonConfigurationGroups.MONSTER_TARGET)]
    [ConfigurationConditional(name: nameof(IsTargetingEnabled), withValue: true)]
    public Observable<bool> ShowOnlyTarget { get; set; } = false;
    #endregion

    #region Filter Settings
    [ConfigurationProperty("MONSTER_WIDGET_DETAILS_CONFIGURATIONS_STRING", group: CommonConfigurationGroups.MONSTER_FILTERS)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public virtual MonsterDetailsConfiguration Details { get; set; } = new();
    #endregion

    [ConfigurationProperty("MONSTER_WIDGET_PARTS_AILMENTS_STACKING_STRING", group: CommonConfigurationGroups.CUSTOMIZATIONS)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<Orientation> PartsAilmentsOrientation { get; set; } = Enums.Orientation.Vertical;

    #region Part Settings
    [ConfigurationProperty("MONSTER_WIDGET_ENABLE_PARTS_STRING", group: CommonConfigurationGroups.MONSTER_PARTS)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<bool> EnableParts { get; set; } = true;

    [ConfigurationProperty("MONSTER_WIDGET_PARTS_COLUMNS_STRING", group: CommonConfigurationGroups.MONSTER_PARTS)]
    [ConfigurationConditional(name: nameof(EnableParts), withValue: true)]
    public Range PartColumns { get; set; } = new(3, 5, 1, 1);

    [ConfigurationProperty("MONSTER_WIDGET_HIDE_UNKNOWN_PARTS_STRING", group: CommonConfigurationGroups.MONSTER_PARTS)]
    [ConfigurationConditional(name: nameof(EnableParts), withValue: true)]
    public Observable<bool> HideUnknownParts { get; set; } = true;

    [ConfigurationProperty("MONSTER_WIDGET_AUTO_HIDE_PARTS_STRING", group: CommonConfigurationGroups.MONSTER_PARTS)]
    [ConfigurationConditional(name: nameof(EnableParts), withValue: true)]
    public Observable<bool> AutoHideParts { get; set; } = true;

    [ConfigurationProperty("MONSTER_WIDGET_AUTO_HIDE_PARTS_DELAY_STRING", group: CommonConfigurationGroups.MONSTER_PARTS)]
    [ConfigurationConditional(name: nameof(AutoHideParts), withValue: true)]
    public Range AutoHidePartsDelay { get; set; } = new(15, 300, 1, 1);
    #endregion

    #region Ailment Settings
    [ConfigurationProperty("MONSTER_WIDGET_ENABLE_AILMENTS_STRING", group: CommonConfigurationGroups.MONSTER_AILMENTS)]
    [ConfigurationConditional(name: nameof(Initialize), withValue: true)]
    public Observable<bool> EnableAilments { get; set; } = true;

    [ConfigurationProperty("MONSTER_WIDGET_AUTO_HIDE_AILMENTS_STRING", group: CommonConfigurationGroups.MONSTER_AILMENTS)]
    [ConfigurationConditional(name: nameof(EnableAilments), withValue: true)]
    public Observable<bool> AutomaticallyHideAilments { get; set; } = true;

    [ConfigurationProperty("MONSTER_WIDGET_AUTO_HIDE_AILMENTS_DELAY_STRING", group: CommonConfigurationGroups.MONSTER_AILMENTS)]
    [ConfigurationConditional(name: nameof(AutomaticallyHideAilments), withValue: true)]
    public Range AutoHideAilmentsDelay { get; set; } = new(15, 300, 1, 1);
    #endregion
}