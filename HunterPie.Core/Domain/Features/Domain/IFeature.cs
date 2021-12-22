using HunterPie.Core.Architecture;

namespace HunterPie.Core.Domain.Features.Domain
{
    public interface IFeature
    {

        public Observable<bool> IsEnabled { get; set; }

        public void Enable();
        public void Disable();
    }
}
