using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Monster;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsTargetKey
{
    [FieldOffset(0x0)] public int Type;
    [FieldOffset(0x4)] public int Key;
}