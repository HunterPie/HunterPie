using System;

namespace HunterPie.Core.Domain.Enums
{
    [Flags]
    public enum GameProcess : int
    {
        None = 0,
        MonsterHunterRise = 1 << 0,
        MonsterHunterWorld = 1 << 1,
        MonsterHunterRiseSunbreakDemo = 1 << 2,
        All = MonsterHunterRise | MonsterHunterWorld | MonsterHunterRiseSunbreakDemo,
    }
}
