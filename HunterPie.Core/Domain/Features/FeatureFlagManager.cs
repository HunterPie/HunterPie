using HunterPie.Core.Domain.Features.Domain;

namespace HunterPie.Core.Domain.Features
{
    internal class FeatureFlagManager
    {
        private readonly IFeatureFlagRepository flagRepository;

        public FeatureFlagManager(IFeatureFlagRepository repository)
        {
            flagRepository = repository;
        }

        public bool IsEnabled(string feature) => flagRepository.IsEnabled(feature);

        #nullable enable
        public IFeature? Get(string feature) => flagRepository.GetFeature(feature);
        #nullable disable
    }
}
