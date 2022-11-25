namespace HunterPie.Core.Domain.Cache.Model;
internal class ThreadSafeCacheEntry
{
    public int Count { get; set; }
    public object Value { get; init; }
}
