using System;

namespace HunterPie.Core.Game.Events;

public class CounterChangeEventArgs(int current, int max) : EventArgs
{
    public int Current { get; } = current;
    public int Max { get; } = max;
}