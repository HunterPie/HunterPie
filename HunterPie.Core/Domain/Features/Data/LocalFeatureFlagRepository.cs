using HunterPie.Core.Domain.Features.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Core.Domain.Features.Data
{
    internal class LocalFeatureFlagRepository : IFeatureFlagRepository
    {

        public Dictionary<string, IFeature> _features = new()
        {

        };

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
