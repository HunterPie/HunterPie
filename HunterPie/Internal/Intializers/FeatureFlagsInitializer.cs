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
    internal class FeatureFlagsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DefaultFeatureFlags);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Dictionary<string, IFeature> features = existingValue as Dictionary<string, IFeature>;


            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
             throw new NotImplementedException();
        }
    }

    public class DefaultFeatureFlags
    {
        private Dictionary<string, IFeature> defaultFeatures = new()
        {
            { FeatureFlags.FEATURE_NATIVE_LOGGER, new NativeLoggerFeature() },
        };

        public IReadOnlyDictionary<string, IFeature> Flags => defaultFeatures;
    }

    internal class FeatureFlagsInitializer : IInitializer
    {
        
        public readonly static DefaultFeatureFlags Features = new();

        public void Init()
        {
            IFeatureFlagRepository localRepository = new LocalFeatureFlagRepository(Features.Flags);
            
            ConfigManager.Register("feature-flags.json", Features.Flags);

            FeatureFlagManager.Initialize(localRepository);
        }
    }
}
