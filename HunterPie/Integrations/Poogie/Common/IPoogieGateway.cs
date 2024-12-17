using HunterPie.Core.Networking.Http;
using HunterPie.Integrations.Poogie.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Common;

internal interface IPoogieClient
{
    public Task<PoogieResult<T>> Get<T>(string path, Dictionary<string, object>? query = null);
    public Task<HttpClientResponse> Download(string path);
    public Task<PoogieResult<TOut>> Post<TIn, TOut>(string path, TIn payload);
    public Task<PoogieResult<T>> SendFile<T>(string path, string filename);
    public Task<PoogieResult<TOut>> Delete<TIn, TOut>(string path, TIn payload);
    public Task<PoogieResult<TOut>> Patch<TIn, TOut>(string path, TIn payload);
}