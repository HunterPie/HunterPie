using System.Net.Http;
using System.Threading.Tasks;

namespace HunterPie.Core.Networking.Http.Intercept;

public interface IHttpChain
{
    public Task<HttpResponseMessage> NextAsync(HttpRequestMessage request);
}
