using HunterPie.Core.Client.Configuration.Debug;
using HunterPie.Core.Client.Configuration.Games;
using HunterPie.Core.Client.Configuration.Overlay;

namespace HunterPie.Core.Client.Configuration;

public interface IConfiguration
{
    public ClientConfig Client { get; }

    public MHRConfig Rise { get; }

    public MHWConfig World { get; }

    public MHWildsConfig Wilds { get; }

    public OverlayClientConfig Overlay { get; }

    public DevelopmentConfig Development { get; }
}
