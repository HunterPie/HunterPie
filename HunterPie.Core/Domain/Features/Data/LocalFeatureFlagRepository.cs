using HunterPie.Core.Domain.Features.Domain;
using System.Collections.Generic;

namespace HunterPie.Core.Domain.Features.Data
{
    
    internal class LocalFeatureFlagRepository : IFeatureFlagRepository
    {

        public readonly IReadOnlyDictionary<string, IFeature> _features;

        public LocalFeatureFlagRepository(IReadOnlyDictionary<string, IFeature> source)
        {
            _features = source;
        }

        public IFeature GetFeature(string feature)
        {
            if (!_features.ContainsKey(feature))
                return null;

            return _features[feature];
        }

        public bool IsEnabled(string feature)
        {
            if (!_features.ContainsKey(feature))
                return false;

            return _features[feature].IsEnabled;
        }
    }
}
