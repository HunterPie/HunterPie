using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Monster;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsPartBreak
{
    [FieldOffset(0x10)] public int MaxBreaks;
    [FieldOffset(0x14)] public int HealthMultiplier;
    [FieldOffset(0x18)] public int Breaks;
    [FieldOffset(0x31)]
    [MarshalAs(UnmanagedType.I1)] public bool IsSeverable;
    [FieldOffset(0x32)]
    [MarshalAs(UnmanagedType.I1)] public bool IsEnabled;
}