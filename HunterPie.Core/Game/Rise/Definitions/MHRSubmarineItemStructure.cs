using System.Runtime.InteropServices;

namespace HunterPie.Core.Game.Rise.Definitions
{
    [StructLayout(LayoutKind.Explicit)]
    public struct MHRSubmarineItemStructure
    {
        [FieldOffset(0x8)]
        public int State;

        [FieldOffset(0x40)]
        public byte Unk0;

        [FieldOffset(0x41)]
        public byte Unk1;

        [FieldOffset(0x42)]
        public byte Unk2;

        [FieldOffset(0x44)]
        public int Amount;

        public bool IsNotEmpty() => Amount > 0 || (Unk0 + Unk1 + Unk2 != 0);
    }
}
