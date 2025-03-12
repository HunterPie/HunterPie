using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Monster;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsBuildUp
{
    [FieldOffset(0x14)] public float Current;
    [FieldOffset(0x1C)] public float Max;
}