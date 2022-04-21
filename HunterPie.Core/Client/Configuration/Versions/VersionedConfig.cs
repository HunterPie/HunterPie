using HunterPie.Core.Domain.Interfaces;

namespace HunterPie.Core.Client.Configuration.Versions
{
    public class VersionedConfig : IVersionedConfig
    {
        private readonly int _version = 0;
        public int Version => _version;

        public VersionedConfig(int version) => _version = version;
    }
}
