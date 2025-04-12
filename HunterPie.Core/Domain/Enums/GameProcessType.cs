﻿using System;

namespace HunterPie.Core.Domain.Enums;

[Flags]
public enum GameProcessType : int
{
    None = 0,
    MonsterHunterRise = 1 << 0,
    MonsterHunterWorld = 1 << 1,
    MonsterHunterWilds = 1 << 2,
    All = MonsterHunterRise | MonsterHunterWorld | MonsterHunterWilds,
}