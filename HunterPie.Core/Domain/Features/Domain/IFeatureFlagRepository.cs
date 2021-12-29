namespace HunterPie.Core.Domain.Features.Domain
{
    internal interface IFeatureFlagRepository
    {
        public IFeature GetFeature(string feature);
        public bool IsEnabled(string feature);
    }
}
