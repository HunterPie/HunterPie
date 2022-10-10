namespace HunterPie.Core.Game.Rise.Definitions
{
    public struct MHRWirebugStructure
    {
        public long Ref; // 0
        public int Unk0; // 80
        public int Unk1; // 8 + 4 
        public float Cooldown;  // _RecastTimer
        public float MaxCooldown; // _RecastTimerMax
        public long Unk2;  // _RecoverWaitTimer
    }

    public struct MHRWirebugExtrasStructure
    {
        public float Timer;
    }
}
