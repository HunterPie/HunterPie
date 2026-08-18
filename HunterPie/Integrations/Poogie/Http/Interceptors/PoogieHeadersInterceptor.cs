using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Networking.Http.Intercept;
using System.Net.Http;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Http.Interceptors;

internal class PoogieHeadersInterceptor(
    ILocalRegistry registry
) : IHttpInterceptor
{
    private const string ClientIdKey = "client_id";
    private const string DefaultClientId = "Unknown";
    private const string ClientIdHeader = "X-Client-Id";

    public Task<HttpResponseMessage> InterceptAsync(HttpRequestMessage request, IHttpChain chain)
    {
        string clientId = registry.Exists(ClientIdKey)
            ? registry.Get<string>(ClientIdKey) ?? DefaultClientId
            : DefaultClientId;

        request.Headers.Add(ClientIdHeader, clientId);

        return chain.NextAsync(request);
    }
}
