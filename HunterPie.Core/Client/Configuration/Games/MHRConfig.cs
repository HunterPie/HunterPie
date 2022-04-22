using HunterPie.Core.Client.Configuration.Integrations;

namespace HunterPie.Core.Client.Configuration.Games
{
    public class MHRConfig
    {
        public DiscordRichPresence RichPresence { get; set; } = new();
        public OverlayConfig Overlay { get; set; } = new();
    }
}
