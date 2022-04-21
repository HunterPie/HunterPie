using HunterPie.Core.Client.Configuration.Integrations;
using HunterPie.Core.Settings;

namespace HunterPie.Core.Client.Configuration.Games
{
    [SettingsGroup("OVERLAY_STRING", "ICON_OVERLAY")]
    public class MHWConfig : ISettings
    {
        public DiscordRichPresence RichPresence { get; set; } = new();
        public OverlayConfig Overlay { get; set; } = new();
    }
}
