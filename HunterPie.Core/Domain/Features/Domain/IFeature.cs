using HunterPie.Core.Architecture;

namespace HunterPie.Core.Domain.Features.Domain
{
    public interface IFeature
    {
        // TODO: Fix this interface to make it easier to implement
        public Observable<bool> IsEnabled { get; set; }

        public void Enable();
        public void Disable();
    }
}
