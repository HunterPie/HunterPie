using HunterPie.Core.Client;
using HunterPie.Core.Domain.Features;
using HunterPie.Core.Domain.Features.Data;
using HunterPie.Core.Domain.Features.Domain;
using HunterPie.Domain.Interfaces;
using HunterPie.Features;

namespace HunterPie.Internal.Initializers
{
    internal class FeatureFlagsInitializer : IInitializer
    {
        
        public readonly static DefaultFeatureFlags Features = new();

        public void Init()
        {
            IFeatureFlagRepository localRepository = new LocalFeatureFlagRepository(Features.ReadOnlyFlags);
            
            ConfigManager.Register("internal/feature-flags.json", Features.Flags);

            FeatureFlagManager.Initialize(localRepository);
        }
    }
}
