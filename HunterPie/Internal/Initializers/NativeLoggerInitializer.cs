using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Domain.Features;
using HunterPie.Core.Logger;
using HunterPie.Domain.Interfaces;
using HunterPie.Internal.Logger;

namespace HunterPie.Internal.Initializers
{
    internal class NativeLoggerInitializer : IInitializer
    {
        public void Init()
        {
            if (FeatureFlagManager.IsEnabled(FeatureFlags.FEATURE_NATIVE_LOGGER))
            {
                ILogger logger = new NativeLogger();
                Log.Add(logger);

                Log.Info("Hello world! HunterPie stdout has been initialized!");
            }
        }
    }
}
