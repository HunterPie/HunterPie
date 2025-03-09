using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Game;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Monster;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsAilment
{
    [FieldOffset(0x28)] public nint BuildUpPointer;
    [FieldOffset(0x54)] public int IsActive;
    [FieldOffset(0x58)] public MHWildsTimer Timer;
    [FieldOffset(0xB4)] public int Id;
}