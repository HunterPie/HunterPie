using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHRSwitchAxeStructure
{
    [FieldOffset(0xA70)] public int BuildUp;
    [FieldOffset(0xA7C)] public float ChargeBuildUp;
    [FieldOffset(0xA84)] public float ChargeTimer;
    [FieldOffset(0xA88)] public float SwitchChargerTimer;
    [FieldOffset(0xA8C)] public float SlamBuffTimer;
}