using HunterPie.Core.Game.Entity.Enemy;
using System;

namespace HunterPie.Core.Game.Services.Monster.Events;

#nullable enable
public class InferTargetChangedEventArgs(IMonster? target) : EventArgs
{
    public IMonster? Target { get; init; } = target;
}