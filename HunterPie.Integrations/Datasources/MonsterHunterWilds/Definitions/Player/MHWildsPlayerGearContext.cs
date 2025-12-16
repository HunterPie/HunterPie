using HunterPie.Core.Game.Enums;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Player;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsPlayerGearContext
{
    [FieldOffset(0x38)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)] public int[] Gear;
    [FieldOffset(0xC8)] public Weapon WeaponId;
}