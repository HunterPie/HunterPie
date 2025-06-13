using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Monster;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsMonsterContext
{
    [FieldOffset(0x3DA)] public short Size;

    // Nullable structure
    [FieldOffset(0x3D4)][MarshalAs(UnmanagedType.I1)] public bool HasFixedSize;
    [FieldOffset(0x3D6)] public short FixedSize;
}