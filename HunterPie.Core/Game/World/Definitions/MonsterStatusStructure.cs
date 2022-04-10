using System.Runtime.InteropServices;

namespace HunterPie.Core.Game.World.Definitions
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MonsterStatusStructure
    {
        public long Reference;
        public long unk0;
        public int unk1;
        public int IsActive;
        public float BuildUp;
        public float DamageDone;
        public float unk3;
        public float Duration;
        public float MaxDuration;
        public int unk4;
        public int unk5;
        public int Counter;
        public int unk6;
        public float MaxBuildup;
        public float unk8; // 10 for legiana
        public float unk9; // 10 for legiana
    }
}
