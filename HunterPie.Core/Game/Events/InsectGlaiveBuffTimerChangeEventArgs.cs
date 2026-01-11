using System;

namespace HunterPie.Core.Game.Events;

public class InsectGlaiveBuffTimerChangeEventArgs(float timer) : EventArgs
{

    public float Timer { get; } = timer;
}