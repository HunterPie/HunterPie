using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Activities;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsSupportShipContext
{
    [FieldOffset(0x23)] public byte Days;
    [FieldOffset(0x24)] public byte InTown;
}