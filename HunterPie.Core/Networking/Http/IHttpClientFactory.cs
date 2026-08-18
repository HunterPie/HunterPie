using HunterPie.Core.Networking.Http.Intercept;

namespace HunterPie.Core.Networking.Http;

public interface IHttpClientFactory
{
    public IHttpClient Create(
        string[] urls,
        int retry,
        IHttpInterceptor[] interceptors
    );
}
