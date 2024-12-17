using HunterPie.Core.Domain.Cache.Model;
using System.Threading.Tasks;

namespace HunterPie.Core.Domain.Cache;

public interface IAsyncCache
{
    public Task<T?> Get<T>(string key);
    public Task Set<T>(string key, T value, CacheOptions? options = null) where T : notnull;
}