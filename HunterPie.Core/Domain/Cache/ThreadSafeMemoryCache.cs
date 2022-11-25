using HunterPie.Core.Domain.Cache.Model;
using HunterPie.Core.Domain.Memory;
using System.Collections.Generic;

namespace HunterPie.Core.Domain.Cache;

internal class ThreadSafeMemoryCache : IMemoryCache
{

    private readonly object _lock = new();
    private readonly Dictionary<long, ThreadSafeCacheEntry> _cache = new();

    public T? Get<T>(long address) where T : struct
    {
        lock (_lock)
        {
            bool found = _cache.TryGetValue(address, out ThreadSafeCacheEntry entry);

            if (!found || entry.Value is not T value)
                return null;

            entry.Count--;

            if (entry.Count <= 0)
                _cache.Remove(address);

            return value;
        }
    }

    public void Set<T>(long address, T value, int nPasses)
    {
        lock (_lock)
            _cache[address] = new ThreadSafeCacheEntry { Count = nPasses, Value = value };
    }
}
