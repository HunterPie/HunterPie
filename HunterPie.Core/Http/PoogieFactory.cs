using HunterPie.Core.Client;
using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Domain.Features;

namespace HunterPie.Core.Http
{
    public static class PoogieFactory
    {

        public static string[] Hosts =
        {
            "https://api.hunterpie.com",
            "https://mirror.hunterpie.com/mirror"
        };

        public static PoogieBuilder Default()
        {
            if (FeatureFlagManager.IsEnabled(FeatureFlags.FEATURE_REDIRECT_POOGIE))
                return new PoogieBuilder(ClientConfig.Config.Debug.PoogieApiHost);

            return new PoogieBuilder(Hosts);
        }
    }
}
