using HunterPie.Core.Game.Client;
using System;

namespace HunterPie.Core.Game.Events;
public class StaminaChangeEventArgs : EventArgs
{

    public double Stamina { get; }
    public double MaxStamina { get; }
    public double MaxRecoverableStamina { get; }
    public double MaxPossibleStamina { get; }

    public StaminaChangeEventArgs(IPlayer player)
    {
        Stamina = player.Stamina;
        MaxStamina = player.MaxStamina;
        MaxRecoverableStamina = player.MaxRecoverableStamina;
        MaxPossibleStamina = player.MaxPossibleStamina;
    }
}
