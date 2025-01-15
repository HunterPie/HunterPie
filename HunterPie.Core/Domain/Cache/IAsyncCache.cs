using HunterPie.Core.Domain.Cache.Model;
using System.Threading.Tasks;

namespace HunterPie.Core.Domain.Cache;

#nullable enable
public interface IAsyncCache
{
    public Task<T?> Get<T>(string key);
    public Task Set<T>(string key, T value, CacheOptions? options = null);
}