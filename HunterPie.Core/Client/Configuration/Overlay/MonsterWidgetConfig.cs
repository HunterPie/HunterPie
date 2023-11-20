using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Adapters;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Annotations;
using HunterPie.Core.Settings.Common;
using HunterPie.Core.Settings.Types;

namespace HunterPie.Core.Client.Configuration.Overlay;

[Configuration("MONSTER_WIDGET_STRING", "ICON_SKULL")]
public class MonsterWidgetConfig : IWidgetSettings, ISettings
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

    [ConfigurationProperty("ORIENTATION_STRING", group: CommonConfigurationGroups.WIDGET)]
    public Observable<Orientation> Orientation { get; set; } = Enums.Orientation.Vertical;

    [ConfigurationProperty("MONSTER_WIDGET_DYNAMIC_RESIZE_STRING", group: CommonConfigurationGroups.WIDGET)]
    public Observable<bool> DynamicResize { get; set; } = false;

    [ConfigurationProperty("MONSTER_WIDGET_MAX_WIDTH_STRING", group: CommonConfigurationGroups.WIDGET)]
    public Range MaxWidth { get; set; } = new(600, 1000, 200, 1);

    [ConfigurationProperty("MONSTER_WIDGET_MIN_WIDTH_STRING", group: CommonConfigurationGroups.WIDGET)]
    public Range MinWidth { get; set; } = new(400, 600, 200, 1);

    [ConfigurationProperty("MONSTER_WIDGET_TARGET_MODE_STRING", availableGames: GameProcess.MonsterHunterRise | GameProcess.MonsterHunterWorld, group: CommonConfigurationGroups.MONSTER_TARGET)]
    [GameConfigurationAdapter(typeof(TargetModeEnumAdapter))]
    public Observable<TargetModeType> TargetMode { get; set; } = TargetModeType.LockOn;

    [ConfigurationProperty("MONSTER_WIDGET_SHOW_ONLY_TARGET_STRING", group: CommonConfigurationGroups.MONSTER_TARGET)]
    public Observable<bool> ShowOnlyTarget { get; set; } = false;

    [ConfigurationProperty("ENABLE_WIDGET_STRING", group: CommonConfigurationGroups.MONSTER_PARTS)]
    public Observable<bool> EnableParts { get; set; } = true;

    [ConfigurationProperty("ENABLE_WIDGET_STRING", group: CommonConfigurationGroups.MONSTER_PARTS)]
    public Observable<bool> HideUnknownParts { get; set; } = true;

    [ConfigurationProperty("ENABLE_WIDGET_STRING", group: CommonConfigurationGroups.MONSTER_PARTS)]
    public Observable<bool> AutoHideParts { get; set; } = true;

    [ConfigurationProperty("ENABLE_WIDGET_STRING", group: CommonConfigurationGroups.MONSTER_PARTS)]
    public Range AutoHidePartsDelay { get; set; } = new(15, 300, 1, 1);

    [ConfigurationProperty("ENABLE_WIDGET_STRING", group: CommonConfigurationGroups.MONSTER_AILMENTS)]
    public Observable<bool> EnableAilments { get; set; } = true;

    [ConfigurationProperty("ENABLE_WIDGET_STRING", group: CommonConfigurationGroups.MONSTER_AILMENTS)]
    public Observable<bool> AutomaticallyHideAilments { get; set; } = true;

    [ConfigurationProperty("ENABLE_WIDGET_STRING", group: CommonConfigurationGroups.MONSTER_AILMENTS)]
    public Range AutoHideAilmentsDelay { get; set; } = new(15, 300, 1, 1);

    [ConfigurationProperty("ENABLE_WIDGET_STRING", group: CommonConfigurationGroups.WIDGET)]
    public Observable<bool> StreamerMode { get; set; } = false;

    [ConfigurationProperty("MONSTER_WIDGET_ENABLE_STAMINA_STRING", group: CommonConfigurationGroups.CUSTOMIZATIONS)]
    public Observable<bool> EnableStamina { get; set; } = true;

    [ConfigurationProperty("ENABLE_WIDGET_STRING", group: CommonConfigurationGroups.WIDGET)]
    public Position Position { get; set; } = new(1200, 100);
}
