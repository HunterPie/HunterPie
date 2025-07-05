using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Monster;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsMonsterContext
{
    [FieldOffset(0x3E2)] public short Size;

    // Nullable structure
    [FieldOffset(0x3DC)][MarshalAs(UnmanagedType.I1)] public bool HasFixedSize;
    [FieldOffset(0x3DE)] public short FixedSize;
}