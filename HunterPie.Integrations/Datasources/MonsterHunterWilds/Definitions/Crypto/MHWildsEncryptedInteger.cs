using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Crypto;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsEncryptedInteger
{
    [FieldOffset(0x0)] public long Value;
    [FieldOffset(0x8)] public long Divisor;

    public long Decode()
    {
        if (Divisor == 0)
            return 0;

        return Value / Divisor;
    }
}