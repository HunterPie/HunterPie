using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Player;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsDamageHistory
{
    [FieldOffset(0x10)] public nint Elements;
    [FieldOffset(0x18)] public int Size;
}