using System.Runtime.InteropServices;

namespace HunterPie.Core.Game.Rise.Definitions
{
    [StructLayout(LayoutKind.Explicit)]
    public struct MHRSubmarineItemStructure
    {
        [FieldOffset(0x44)]
        public int Amount;
    }
}
