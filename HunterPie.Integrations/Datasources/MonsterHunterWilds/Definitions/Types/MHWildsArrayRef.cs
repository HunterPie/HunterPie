using HunterPie.Core.Domain.Memory;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Types;

public readonly struct MHWildsArrayRef<T>(nint address) where T : struct
{
    public readonly nint Address = address;

    public async Task<T> ElementAt(IMemoryAsync memory, int index)
    {
        nint reference = await memory.ReadAsync<nint>(Address + (index * sizeof(long)) + 0x20);
        return await memory.ReadAsync<T>(reference);
    }
}