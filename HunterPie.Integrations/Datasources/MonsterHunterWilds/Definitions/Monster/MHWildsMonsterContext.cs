using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Types;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Monster;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsMonsterContext
{
    [FieldOffset(0x30)] public MHWildsVector3 Position;

    // Nullable structure
    [FieldOffset(0x3E8)][MarshalAs(UnmanagedType.I1)] public bool HasFixedSize;
    [FieldOffset(0x3EA)] public short FixedSize;

    [FieldOffset(0x3EC)] public short Size;
}