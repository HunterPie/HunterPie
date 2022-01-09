using HunterPie.Core.Client.Configuration.Debug;

namespace HunterPie.Core.Client.Configuration
{
    public class Config
    {
        public ClientConfig Client { get; set; } = new ClientConfig();
        public OverlayConfig Overlay { get; set; } = new OverlayConfig();
        public DevelopmentConfig Debug { get; set; } = new DevelopmentConfig();
    }
}
