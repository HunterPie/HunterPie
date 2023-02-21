using HunterPie.Core.Architecture;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Types;

namespace HunterPie.Core.Client.Configuration.Overlay;

[SettingsGroup("OVERLAY_STRING", "ICON_OVERLAY")]
public class OverlayClientConfig : ISettings
{
    [SettingField("OVERLAY_ENABLED_STRING")]
    public Observable<bool> IsEnabled { get; set; } = true;

    [SettingField("OVERLAY_KEYBINDING_TOGGLE_VISIBILITY_STRING")]
    public Keybinding ToggleVisibility { get; set; } = "Ctrl+Alt+O";

    [SettingField("OVERLAY_KEYBINDING_TOGGLE_DESIGN_MODE", requiresRestart: true)]
    public Keybinding ToggleDesignMode { get; set; } = "ScrollLock";

    [SettingField("OVERLAY_HIDE_WHEN_GAME_UNFOCUS_STRING")]
    public Observable<bool> HideWhenUnfocus { get; set; } = false;
}
