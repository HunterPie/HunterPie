using HunterPie.Core.Architecture;
using HunterPie.Core.Domain.Features.Domain;

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

        public void Disable() {}
        public void Enable() {}
    }
}
