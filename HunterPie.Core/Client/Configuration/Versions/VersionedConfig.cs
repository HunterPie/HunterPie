using HunterPie.Core.Domain.Interfaces;

namespace HunterPie.Core.Client.Configuration.Versions;

public class VersionedConfig : IVersionedConfig
{
    public int Version { get; } = 0;

    public VersionedConfig(int version)
    {
        Version = version;
    }
}
