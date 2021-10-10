using System.Runtime.InteropServices;

namespace HunterPie.Core.Game.Client.Data.Definitions
{
    [StructLayout(LayoutKind.Sequential, Size = 0x40)]
    public struct SHealingData
    {
        public ulong Ref1;
        public ulong Ref2;
        public float CurrentHeal;
        public float OldMaxHeal;
        public float MaxHeal;
        public float CurrentHealSpeed;
        public float MaxHealSpeed;
        public float unk;
        public float unk1;
        public int Stage;
        public int unk2;
        public int unk3;
        public int unk4;
        public int unk5;
    }
}
