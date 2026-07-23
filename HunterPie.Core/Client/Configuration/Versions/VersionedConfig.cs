using HunterPie.Core.Domain.Interfaces;

namespace HunterPie.Core.Client.Configuration.Versions;

public class VersionedConfig(int version) : IVersionedConfig
{
    public int Version { get; } = version;
}