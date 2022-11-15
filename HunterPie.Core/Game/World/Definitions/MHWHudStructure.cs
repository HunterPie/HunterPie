using System.Runtime.InteropServices;

namespace HunterPie.Core.Game.World.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHWHudStructure
{
    [FieldOffset(0x30)]
    public long HealingArrayPointer;

    [FieldOffset(0x60)]
    public float MaxHealth;

    [FieldOffset(0x64)]
    public float Health;

    [FieldOffset(0x13C)]
    public float Stamina;

    [FieldOffset(0x144)]
    public float MaxStamina;

    [FieldOffset(0x2DE4)]
    public float RecoverableHealth;
}
