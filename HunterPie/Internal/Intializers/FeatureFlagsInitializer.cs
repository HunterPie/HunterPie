using HunterPie.Core.Client;
using HunterPie.Core.Domain.Features;
using HunterPie.Core.Domain.Features.Data;
using HunterPie.Core.Domain.Features.Domain;
using HunterPie.Domain.Constants;
using HunterPie.Domain.Interfaces;
using HunterPie.Internal.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HunterPie.Internal.Intializers
{
    public class DefaultFeatureFlags
    {
        internal readonly Dictionary<string, IFeature> Flags = new()
        {
            { FeatureFlags.FEATURE_NATIVE_LOGGER, new NativeLoggerFeature() },
            { FeatureFlags.FEATURE_METRICS_WIDGET, new Feature() },
        };

        public IReadOnlyDictionary<string, IFeature> ReadOnlyFlags => Flags;
    }

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
