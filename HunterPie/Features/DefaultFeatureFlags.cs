using HunterPie.Core.Domain.Features.Domain;
using HunterPie.Domain.Constants;
using HunterPie.Internal.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Features
{
    internal class DefaultFeatureFlags
    {
        internal readonly Dictionary<string, IFeature> Flags = new()
        {
            { FeatureFlags.FEATURE_NATIVE_LOGGER, new NativeLoggerFeature() },
            { FeatureFlags.FEATURE_METRICS_WIDGET, new Feature() },
            { FeatureFlags.FEATURE_USER_ACCOUNT, new Feature() },
        };

        public IReadOnlyDictionary<string, IFeature> ReadOnlyFlags => Flags;
    }
}
