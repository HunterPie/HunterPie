using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Activities;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsMaterialCollector
{
    [FieldOffset(0x10)] public nint ItemsPointer;
    [FieldOffset(0x2C)] public uint Id;
}