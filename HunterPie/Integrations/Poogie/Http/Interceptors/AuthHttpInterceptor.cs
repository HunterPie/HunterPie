using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Networking.Http.Intercept;
using HunterPie.Core.Vault;
using System.Net.Http;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Http.Interceptors;

internal class AuthHttpInterceptor(
    ICredentialVault vault,
    IConfiguration config
) : IHttpInterceptor
{
    private const string TokenHeader = "X-Token";
    private const string SupporterHeader = "X-Supporter-Token";


    public Task<HttpResponseMessage> InterceptAsync(HttpRequestMessage request, IHttpChain chain)
    {
        request.Headers.Add(SupporterHeader, config.Client.SupporterSecretToken);

        if (vault.Get() is { } credential)
            request.Headers.Add(TokenHeader, credential.Password);

        return chain.NextAsync(request);
    }
}
