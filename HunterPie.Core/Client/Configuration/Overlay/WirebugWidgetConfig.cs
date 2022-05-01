using HunterPie.Core.Architecture;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Types;

namespace HunterPie.Core.Client.Configuration.Overlay
{
    [SettingsGroup("WIREBUG_WIDGET_STRING", "ICON_WIREBUG_DARK", availableGames: GameProcess.MonsterHunterRise)]
    public class WirebugWidgetConfig : IWidgetSettings, ISettings
    {
        [SettingField("INITIALIZE_WIDGET_STRING", requiresRestart: true)]
        public Observable<bool> Initialize { get; set; } = true;

        [SettingField("ENABLE_WIDGET_STRING")]
        public Observable<bool> Enabled { get; set; } = true;

        [SettingField("HIDE_WHEN_UI_VISIBLE_STRING")]
        public Observable<bool> HideWhenUiOpen { get; set; } = false;

        [SettingField("WIDGET_OPACITY")]
        public Range Opacity { get; set; } = new(1, 1, 0.1, 0.1);

        [SettingField("WIDGET_SCALE")]
        public Range Scale { get; set; } = new(1, 2, 0.1, 0.1);

        [SettingField("WIREBUG_WIDGET_PATCH_IN_GAME_HUD_STRING", requiresRestart: true)]
        public Observable<bool> PatchInGameHud { get; set; } = false;

        [SettingField("ENABLE_STREAMER_MODE")]
        public Observable<bool> StreamerMode { get; set; } = false;

        [SettingField("WIDGET_POSITION")]
        public Position Position { get; set; } = new(840, 920);
    }
}
