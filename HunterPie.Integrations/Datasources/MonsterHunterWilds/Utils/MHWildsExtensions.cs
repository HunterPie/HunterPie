using HunterPie.Core.Domain.Memory;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Collections;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Utils;

public static class MHWildsExtensions
{
    public static async Task<nint[]> ReadDynamicArraySafeAsync(this IMemoryAsync memory, nint address, int count)
    {
        MHWildsDynamicArray structure = await memory.ReadAsync<MHWildsDynamicArray>(address);
        count = Math.Min(count, structure.Count);

        return await memory.ReadAsync<nint>(structure.Elements + 0x20, count);
    }

    public static async Task<T[]> ReadArraySafeAsync<T>(this IMemoryAsync memory, nint address, int count) where T : struct
    {
        int size = await memory.ReadAsync<int>(address + 0x1C);

        return await memory.ReadAsync<T>(address + 0x20, size);
    }

    public static async IAsyncEnumerable<T> ReadArrayOfPtrsSafeAsync<T>(this IMemoryAsync memory, nint address, int count)
        where T : struct
    {
        nint[] pointers = await memory.ReadArraySafeAsync<nint>(address, count);

        foreach (nint pointer in pointers)
            yield return await memory.ReadAsync<T>(pointer);
    }
}