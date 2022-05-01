using System;

namespace HunterPie.Core.Domain.Enums
{
    [Flags]
    public enum GameProcess : int
    {
        None = 0,
        MonsterHunterRise = 1 << 0,
        MonsterHunterWorld = 1 << 1,
    }
}
