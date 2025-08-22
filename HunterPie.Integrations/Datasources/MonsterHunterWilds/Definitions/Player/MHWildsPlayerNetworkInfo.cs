using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Player;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsPlayerNetworkInfo
{
    [FieldOffset(0x28)] public int QuestIndex;
}