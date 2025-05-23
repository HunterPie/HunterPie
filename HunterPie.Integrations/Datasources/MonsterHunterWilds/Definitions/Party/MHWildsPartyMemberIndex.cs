using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Party;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsPartyMemberIndex
{
    [FieldOffset(0x10)] public int Index;
    [FieldOffset(0x14)] public int Padding;
}