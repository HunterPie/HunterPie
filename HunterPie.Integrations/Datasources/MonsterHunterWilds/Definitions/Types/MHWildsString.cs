using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Types;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsString
{
    [FieldOffset(0x10)] public int Length;
    [FieldOffset(0x14)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)] public byte[] Buffer;
}