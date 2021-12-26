using HunterPie.Core.Domain.Features;
using HunterPie.Core.Domain.Features.Data;
using HunterPie.Core.Domain.Features.Domain;
using HunterPie.Domain.Constants;
using HunterPie.Domain.Interfaces;
using HunterPie.Internal.Logger;
using System.Collections.Generic;

namespace HunterPie.Internal.Intializers
{
    internal class FeatureFlagsInitializer : IInitializer
    {

        private static Dictionary<string, IFeature> defaultFeatures = new()
        {
            { FeatureFlags.FEATURE_NATIVE_LOGGER, new NativeLoggerFeature() },
        };

        public static IReadOnlyDictionary<string, IFeature> Features => defaultFeatures;

        public void Init()
        {
            IFeatureFlagRepository localRepository = new LocalFeatureFlagRepository(defaultFeatures);
            
            _ = new FeatureFlagManager(localRepository);
        }
    }
}
