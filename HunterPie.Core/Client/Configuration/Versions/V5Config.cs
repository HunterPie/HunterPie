using HunterPie.Core.Client.Configuration.Debug;
using HunterPie.Core.Client.Configuration.Games;
using HunterPie.Core.Client.Configuration.Overlay;

namespace HunterPie.Core.Client.Configuration.Versions;

public class V5Config : VersionedConfig
{
    public V5Config() : base(4) { }

    public ClientConfig Client { get; set; } = new();

    public MHRConfig Rise { get; set; } = new();

    public MHWConfig World { get; set; } = new();

    public MHWildsConfig Wilds { get; set; } = new();

    public OverlayClientConfig Overlay { get; set; } = new();

    public DevelopmentConfig Development { get; set; } = new();
}