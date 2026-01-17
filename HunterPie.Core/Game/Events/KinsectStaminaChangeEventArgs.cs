using HunterPie.Core.Game.Entity.Player.Classes;
using System;

namespace HunterPie.Core.Game.Events;

public class KinsectStaminaChangeEventArgs(IInsectGlaive source) : EventArgs
{
    public float Current { get; } = source.Stamina;
    public float Max { get; } = source.MaxStamina;
}