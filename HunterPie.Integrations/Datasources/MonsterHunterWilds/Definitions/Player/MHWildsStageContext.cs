using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Player;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsStageContext
{
    [FieldOffset(0xB8)] public int StageId;
    [FieldOffset(0xD5)]
    [MarshalAs(UnmanagedType.I1)] public bool IsSafeZone;
}