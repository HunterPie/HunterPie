using HunterPie.Core.Networking.Http.Models;
using System.Threading.Tasks;

namespace HunterPie.Core.Networking.Http;

public delegate void RequestConfiguration(IRequestOptions options);

public interface IHttpClient
{
    public Task<Response> GetAsync(string path, RequestConfiguration? cfg = null);

    public Task<Response> PostAsync(string path, RequestConfiguration? cfg = null);

    public Task<Response> PatchAsync(string path, RequestConfiguration? cfg = null);

    public Task<Response> PutAsync(string path, RequestConfiguration? cfg = null);

    public Task<Response> DeleteAsync(string path, RequestConfiguration? cfg = null);
}
