using System.Runtime.InteropServices;

namespace HunterPie.Core.Game.Rise.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHRSubmarineItemStructure
{
    [FieldOffset(0x14)]
    public int Amount;

    public bool IsNotEmpty() => Amount > 0;
}
