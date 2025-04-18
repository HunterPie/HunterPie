using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Player;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsStageContext
{
    [FieldOffset(0x7C)] public int StageId;
    [FieldOffset(0xB2)]
    [MarshalAs(UnmanagedType.I1)] public bool IsSafeZone;
}