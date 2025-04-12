using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Types;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsLimitedArray
{
    [FieldOffset(0x10)] public nint Elements;
    [FieldOffset(0x18)] public int Length;
}