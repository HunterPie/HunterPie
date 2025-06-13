﻿using HunterPie.Core.Domain.Memory;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Collections;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Types;
using System.Text;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Utils;

public static class MHWildsExtensions
{
    public static async Task<nint[]> ReadDynamicArraySafeAsync(this IMemoryAsync memory, nint address, int count)
    {
        MHWildsDynamicArray structure = await memory.ReadAsync<MHWildsDynamicArray>(address);
        count = Math.Min(count, structure.Count);
        count = Math.Max(count, 0);

        return await memory.ReadAsync<nint>(structure.Elements + 0x20, count);
    }

    public static async Task<T[]> ReadArraySafeAsync<T>(this IMemoryAsync memory, nint address, int count) where T : struct
    {
        int size = await memory.ReadAsync<int>(address + 0x1C);
        count = Math.Min(size, count);
        count = Math.Max(count, 0);

        return await memory.ReadAsync<T>(address + 0x20, count);
    }

    public static async IAsyncEnumerable<T> ReadArrayOfPtrsSafeAsync<T>(this IMemoryAsync memory, nint address, int count)
        where T : struct
    {
        nint[] pointers = await memory.ReadArraySafeAsync<nint>(address, count);

        foreach (nint pointer in pointers)
            yield return await memory.ReadAsync<T>(pointer);
    }

    public static async Task<T[]> ReadArrayAsync<T>(this IMemoryAsync memory, nint address) where T : struct
    {
        int size = await memory.ReadAsync<int>(address + 0x1C);

        return await memory.ReadAsync<T>(address + 0x20, size);
    }

    public static async IAsyncEnumerable<T> ReadArrayOfPtrsAsync<T>(this IMemoryAsync memory, nint address)
        where T : struct
    {
        nint[] pointers = await memory.ReadArrayAsync<nint>(address);

        foreach (nint pointer in pointers)
            yield return await memory.ReadAsync<T>(pointer);
    }

    public static async Task<string> ReadStringSafeAsync(this IMemoryAsync memory, nint address, int size)
    {
        MHWildsString str = await memory.ReadAsync<MHWildsString>(address);
        size = Math.Max(0, Math.Min(size, str.Length));

        return await memory.ReadAsync(address + 0x14, size * 2, Encoding.Unicode);
    }
}