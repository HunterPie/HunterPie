namespace HunterPie.Core.Game.Rise.Definitions
{
    public struct MHREnrageStructure
    {
        public long Reference;  // AngerMax
        public int unk0;  // Em (Enemy)
        public int unk1;  // <Boolean> AlwaysAnger
        public long unk2;  // <Boolean> NushiAngerMode (Nushi means Apex)
        public int unk3;  // <Boolean> Enable
        public float BuildUp;  // AngerPoint
        public float MaxBuildUp;  // LimitAnger
        public float unk4;  // AddRateExternal
        public float Timer;  // Timer
        public float MaxTimer;  // TimerAnger
        public float unk5;  // ExtendTimeAnger
        public int Counter;  // CountAnger
        // HyakuryuAngerCoolTime (Hyakuryu means Rampage)
        // HyakuryuAngerAddTime
        // StateParam
        // AttrBit
        // SeparateDataIndex
        // IsExistAngerEndAction
    }
}
