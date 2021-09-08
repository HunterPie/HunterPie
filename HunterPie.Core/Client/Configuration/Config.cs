namespace HunterPie.Core.Client.Configuration
{
    public class Config
    {
        public ClientConfig Client { get; set; } = new ClientConfig();
        public OverlayConfig Overlay { get; set; } = new OverlayConfig();
    }
}
