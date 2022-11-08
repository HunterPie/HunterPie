using System.Runtime.InteropServices;

namespace HunterPie.Core.Game.Rise.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHRPetalaceDataStructure
{
    [FieldOffset(0x28)]
    public long Stats;
}
