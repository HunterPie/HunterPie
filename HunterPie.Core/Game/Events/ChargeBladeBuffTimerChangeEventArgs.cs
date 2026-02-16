using System;

namespace HunterPie.Core.Game.Events;

public class ChargeBladeBuffTimerChangeEventArgs(float timer) : EventArgs
{
    public float Timer { get; } = timer;
}