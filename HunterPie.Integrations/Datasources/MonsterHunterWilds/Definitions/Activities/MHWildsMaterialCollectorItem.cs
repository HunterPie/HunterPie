using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Activities;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsMaterialCollectorItem
{
    [FieldOffset(0x10)] public short Id;
    [FieldOffset(0x12)] public short Count;
}