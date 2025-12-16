using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Player;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsStageContext
{
    [FieldOffset(0x88)] public int StageId;
    [FieldOffset(0xC5)]
    [MarshalAs(UnmanagedType.I1)] public bool IsSafeZone;
}