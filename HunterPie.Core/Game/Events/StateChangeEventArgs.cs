using System;

namespace HunterPie.Core.Game.Events;

public class StateChangeEventArgs<T>(T state, T previousState) : EventArgs
{
    public T State { get; } = state;
    public T PreviousState { get; } = previousState;
}