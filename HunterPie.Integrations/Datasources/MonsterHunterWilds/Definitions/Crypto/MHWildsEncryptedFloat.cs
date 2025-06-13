using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Crypto;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsEncryptedFloat
{
    [FieldOffset(0x10)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public ulong[] Parts;

    public Vector128<byte> ToVector128()
    {
        return Vector128.Create(Parts).AsByte();
    }
}