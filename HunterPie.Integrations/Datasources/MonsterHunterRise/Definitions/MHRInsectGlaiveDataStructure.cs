using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHRInsectGlaiveDataStructure
{
    [FieldOffset(0x9F8)] public long ExtractsArray;
    [FieldOffset(0xA34)] public float AttackTimer;
    [FieldOffset(0xA38)] public float SpeedTimer;
    [FieldOffset(0xA3C)] public float DefenseTimer;
}
