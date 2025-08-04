using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Save;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsSaveEquipment
{
    [FieldOffset(0x40)] public int SpecializedToolId;
}