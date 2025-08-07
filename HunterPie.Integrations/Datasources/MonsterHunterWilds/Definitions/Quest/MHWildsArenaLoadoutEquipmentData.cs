using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Quest;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsArenaLoadoutEquipmentData
{
    [FieldOffset(0x50)] public int SpecializedToolId;
}