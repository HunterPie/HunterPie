using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Party;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsPartyArray
{
    [FieldOffset(0x1C)] public int Capacity;
    [FieldOffset(0x20)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public int[]? Members;
}