using HunterPie.Core.Game.Entity.Player.Vitals;
using System;

namespace HunterPie.Core.Game.Events;

public class StaminaChangeEventArgs(IStaminaComponent component) : EventArgs
{

    public double Stamina { get; } = component.Current;
    public double MaxStamina { get; } = component.Max;
    public double MaxRecoverableStamina { get; } = component.MaxRecoverableStamina;
    public double MaxPossibleStamina { get; } = component.MaxPossibleStamina;
}