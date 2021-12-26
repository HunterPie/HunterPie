using HunterPie.Core.Architecture;
using HunterPie.Core.Domain.Features.Domain;
using HunterPie.Domain.Constants;

namespace HunterPie.Internal.Logger
{
    internal class NativeLoggerFeature : IFeature
    {
        Observable<bool> _isEnabled = new(false);
        public Observable<bool> IsEnabled
        {
            get => _isEnabled;
            set => _isEnabled.Value = value;
        }

        public string Name => FeatureFlags.FEATURE_NATIVE_LOGGER;

        public void Disable() {}
        public void Enable() {}
    }
}
