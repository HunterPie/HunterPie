using System;

namespace HunterPie.Core.Game.Events;

public class ValueCreationEventArgs<T>(T value) : EventArgs
{
    public T Value { get; } = value;
}