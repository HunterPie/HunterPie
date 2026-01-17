using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using System;

namespace HunterPie.Core.Game.Events;

public class MonsterTargetEventArgs(IMonster monster) : EventArgs
{
    public Target LockOnTarget { get; init; } = monster.Target;
    public Target ManualTarget { get; init; } = monster.ManualTarget;
}