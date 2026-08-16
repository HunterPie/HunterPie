using System.Net.Http;
using System.Threading.Tasks;

namespace HunterPie.Core.Networking.Http.Intercept;

public interface IHttpInterceptor
{
    public Task<HttpResponseMessage> InterceptAsync(HttpRequestMessage request, IHttpChain chain);
}
