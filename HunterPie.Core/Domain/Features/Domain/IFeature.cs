namespace HunterPie.Core.Domain.Features.Domain
{
    public interface IFeature
    {

        public bool IsEnabled { get; set; }

        public void Enable();
        public void Disable();
    }
}
