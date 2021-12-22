using HunterPie.Core.Domain.Features.Domain;
using System.Collections.Generic;

namespace HunterPie.Core.Domain.Features.Data
{
    
    internal class LocalFeatureFlagRepository : IFeatureFlagRepository
    {

        public Dictionary<string, IFeature> _features = new();

        public LocalFeatureFlagRepository(Dictionary<string, IFeature> source)
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
