using System;

namespace HunterPie.Core.Game.Events;

public class ValueCreationEventArgs<T> : EventArgs
{
    public T Value { get; }

    public ValueCreationEventArgs(T value)
    {
        Value = value;
    }
}