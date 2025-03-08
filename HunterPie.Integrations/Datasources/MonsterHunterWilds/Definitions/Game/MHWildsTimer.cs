using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Game;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsTimer
{
    [FieldOffset(0x0)] public float Current;
    [FieldOffset(0x4)] public float Max;
    [FieldOffset(0xB)]
    [MarshalAs(UnmanagedType.I1)] public bool IsEnabled;
}