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
}