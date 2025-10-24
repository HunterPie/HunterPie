using HunterPie.Core.Game.Enums;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Party;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsNpcCreation
{
    [FieldOffset(0x10)] public int Id;
    [FieldOffset(0x20)] public Weapon Weapon;
}