using HunterPie.Core.Game.Enums;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Party;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsNpcCreation
{
    [FieldOffset(0x24)] public Weapon Weapon;
    [FieldOffset(0x28)] public int Id;
}