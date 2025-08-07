using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace HunterPie.Core.Domain.Memory.Types;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Ref<T> where T : struct
{
    public readonly nint Address;

    public Ref(nint address)
    {
        Address = address;
    }

    public async Task<T> Deref(IMemoryReaderAsync reader)
    {
        return await reader.ReadAsync<T>(Address);
    }
}