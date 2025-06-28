using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Activities;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsIngredientCenterContext
{
    [FieldOffset(0x40)] public float Timer;
    [FieldOffset(0x44)] public short Count;
}