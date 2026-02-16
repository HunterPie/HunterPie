using System;

namespace HunterPie.Core.Game.Events;

public class BuildUpChangeEventArgs(float current, float max) : EventArgs
{
    public float Current { get; } = current;
    public float Max { get; } = max;
}