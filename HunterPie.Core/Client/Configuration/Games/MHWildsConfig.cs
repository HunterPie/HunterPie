using HunterPie.Core.Client.Configuration.Overlay;

namespace HunterPie.Core.Client.Configuration.Games;

public class MHWildsConfig : GameConfig
{
    public override OverlayConfig Overlay { get; set; }
        = new MHWildsOverlayConfig();
}