using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace HunterPie.Core.Domain.Memory.Types;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Ref<T>(nint address) where T : struct
{
    public readonly nint Address = address;

    public async Task<T> Deref(IMemoryReaderAsync reader)
    {
        return await reader.ReadAsync<T>(Address);
    }

    public static implicit operator Ref<T>(nint pointer)
    {
        return new Ref<T>(pointer);
    }
}