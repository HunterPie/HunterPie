namespace HunterPie.Core.Domain.Memory;

/// <summary>
/// Provides a cache for memory scanning to avoid reading the same pointers multiple times
/// </summary>
public interface IMemoryCache
{
    /// <summary>
    /// Fetches the value of a remote memory address
    /// </summary>
    /// <typeparam name="T">Type of memory address</typeparam>
    /// <param name="address">Memory address</param>
    /// <returns>Value contained by that memory address</returns>
    public T? Get<T>(long address) where T : struct;

    /// <summary>
    /// Sets the value of a memory address
    /// </summary>
    /// <typeparam name="T">Type of the value to set</typeparam>
    /// <param name="address">Memory address</param>
    /// <param name="value">Value</param>
    /// <param name="nPasses">Number of passes until it expires</param>
    public void Set<T>(long address, T value, int nPasses);
}
