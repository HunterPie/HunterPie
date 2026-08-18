using HunterPie.Architecture.Http.Client;
using HunterPie.Core.Networking.Http;
using HunterPie.Core.Networking.Http.Intercept;
using NetHttpClient = System.Net.Http.HttpClient;
namespace HunterPie.Architecture.Http.Factory;

internal class DefaultHttpClientFactory(
    NetHttpClient client
) : IHttpClientFactory
{
    public IHttpClient Create(string[] urls, int retry, IHttpInterceptor[] interceptors)
    {
        return new HttpClient(
            client: client,
            urls: urls,
            retry: retry,
            interceptors: interceptors
        );
    }
}
