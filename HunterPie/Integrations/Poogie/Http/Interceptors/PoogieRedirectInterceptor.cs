using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Domain.Features.Repository;
using HunterPie.Core.Networking.Http.Intercept;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Http.Interceptors;

internal class PoogieRedirectInterceptor(
    IFeatureFlagRepository featureFlags,
    IConfiguration config
) : IHttpInterceptor
{
    private readonly bool IsRedirectEnabled = featureFlags.GetFeature(
        feature: FeatureFlags.FEATURE_REDIRECT_POOGIE
    )?.IsEnabled ?? false;

    public Task<HttpResponseMessage> InterceptAsync(HttpRequestMessage request, IHttpChain chain)
    {
        if (!IsRedirectEnabled || request.RequestUri is null)
            return chain.NextAsync(request);

        var redirectBaseUrl = new Uri(config.Development.PoogieApiHost);
        var redirectAbsoluteUrl = new Uri(redirectBaseUrl, request.RequestUri);
        request.RequestUri = redirectAbsoluteUrl;

        return chain.NextAsync(request);
    }
}
