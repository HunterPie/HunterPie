using HunterPie.Core.Architecture;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Annotations;
using HunterPie.Core.Settings.Common;
using HunterPie.Core.Settings.Types;

namespace HunterPie.Core.Client.Configuration.Overlay.Class;

public class ClassWidgetConfig : IWidgetSettings, ISettings
{
    #region General Settings
    [ConfigurationProperty("INITIALIZE_WIDGET_STRING", requiresRestart: true, availableGames: GameProcessType.None, group: CommonConfigurationGroups.GENERAL)]
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
}