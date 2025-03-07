using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Monster;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsMonsterBasicData
{
    [FieldOffset(0x48)] public int Id;
    [FieldOffset(0x54)] public int Category;
}