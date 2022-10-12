using System.Runtime.InteropServices;

namespace HunterPie.Core.Game.Rise.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHRCohootStructure
{
    [FieldOffset(0x18)]
    public int ElgadoCount;

    [FieldOffset(0x28)]
    public int KamuraCount;
}
