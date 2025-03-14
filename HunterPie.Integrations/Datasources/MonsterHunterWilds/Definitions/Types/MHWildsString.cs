using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Types;

[StructLayout(LayoutKind.Explicit, Size = 0x50, Pack = 1)]
public struct MHWildsString
{
    [FieldOffset(0x10)] public int Length;
}