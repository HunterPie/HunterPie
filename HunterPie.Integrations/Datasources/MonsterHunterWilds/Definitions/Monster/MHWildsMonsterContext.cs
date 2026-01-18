using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Types;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Monster;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsMonsterContext
{
    [FieldOffset(0x30)] public MHWildsVector3 Position;

    [FieldOffset(0x3E0)] public short Size;

    // Nullable structure
    [FieldOffset(0x3E4)][MarshalAs(UnmanagedType.I1)] public bool HasFixedSize;
    [FieldOffset(0x3E6)] public short FixedSize;
}