using HunterPie.Core.Domain.Cache.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HunterPie.Core.Domain.Cache;

/// <summary>
/// A thread safe and asynchronous in-memory cache
/// </summary>
public class InMemoryAsyncCache : IAsyncCache
{
    private readonly SemaphoreSlim _semaphore = new(1);
    private readonly Dictionary<string, ThreadSafeCacheEntry> _innerCache = new(50);

    public async Task<T?> Get<T>(string key)
    {
        try
        {
            await _semaphore.WaitAsync();

            bool exists = _innerCache.TryGetValue(key, out ThreadSafeCacheEntry? value);

            if (!exists || value is null)
                return default;

            if (value.ExpiresAt < DateTime.UtcNow)
                return default;

            return value.Value is T typedValue ? typedValue : default;
        }
        catch
        {
            return default;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task Set<T>(string key, T value, CacheOptions? options = null) where T : notnull
    {
        options ??= CacheOptions.Default;

        try
        {
            await _semaphore.WaitAsync();

            _innerCache[key] = new ThreadSafeCacheEntry(
                ExpiresAt: DateTime.UtcNow + options.Ttl,
                Value: value
            );
        }
        catch { }
        finally
        {
            _semaphore.Release();
        }
    }
}