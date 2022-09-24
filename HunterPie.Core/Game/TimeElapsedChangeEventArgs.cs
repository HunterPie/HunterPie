using System;

namespace HunterPie.Core.Game;

public class TimeElapsedChangeEventArgs : EventArgs
{
    public new static readonly TimeElapsedChangeEventArgs Empty = new(false);

    public static readonly TimeElapsedChangeEventArgs TimerReset = new(true);

    public TimeElapsedChangeEventArgs(bool isTimerReset)
    {
        IsTimerReset = isTimerReset;
    }

    /// <summary>
    /// Whether the timer has been reset (and switched) into another zero or some non-zero value.
    /// This indicates the time elapsed may experience sudden change.
    /// </summary>
    public bool IsTimerReset { get; }
}
