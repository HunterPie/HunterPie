using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Collections;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsDynamicArray
{
    [FieldOffset(0x0)] public nint Elements;
    [FieldOffset(0x8)] public int Count;
}