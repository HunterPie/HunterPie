using HunterPie.Core.Domain.Features.Domain;

namespace HunterPie.Core.Domain.Features
{
    internal class FeatureFlagManager
    {
        private IFeatureFlagRepository flagRepository;
        private static FeatureFlagManager _instance;

        public FeatureFlagManager(IFeatureFlagRepository repository)
        {
            if (_instance is not null)
                return;

            _instance = new() { 
                flagRepository = repository    
            };
        }

        private FeatureFlagManager() {}

        public static bool IsEnabled(string feature) => _instance.flagRepository.IsEnabled(feature);

        #nullable enable
        public static IFeature? Get(string feature) => _instance.flagRepository.GetFeature(feature);
        #nullable disable
    }
}
