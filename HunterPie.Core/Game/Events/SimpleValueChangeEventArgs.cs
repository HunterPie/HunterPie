using System;

namespace HunterPie.Core.Game.Events;

public class SimpleValueChangeEventArgs<T>(T oldValue, T newValue) : EventArgs
{
    public T OldValue { get; } = oldValue;
    public T NewValue { get; } = newValue;
}