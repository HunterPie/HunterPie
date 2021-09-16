using System.Runtime.InteropServices;

namespace HunterPie.Core.Game.Definitions
{
    [StructLayout(LayoutKind.Sequential, Size = 160)]
    public struct CMusicSkillData
    {
        public long @base;
        public float Duration;
        public float RebuffDuration; // Duration to be added when you re-use the same skill?
        public int unk0;
        public int unk1;
        public int StringId;
        public int StringIdS;
        public int StringIdM; // Maybe?
        public int StringIdL;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 120)]
        public byte[] unknData;
    }
}
