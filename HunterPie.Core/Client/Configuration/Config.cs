using HunterPie.Core.Client.Configuration.Debug;
using HunterPie.Core.Client.Configuration.Integrations;

namespace HunterPie.Core.Client.Configuration
{
    public class Config
    {
        public ClientConfig Client { get; set; } = new ClientConfig();
        public DiscordRichPresence RichPresence { get; set; } = new DiscordRichPresence();
        public OverlayConfig Overlay { get; set; } = new OverlayConfig();
        public DevelopmentConfig Debug { get; set; } = new DevelopmentConfig();
    }
}
