using System;

namespace HunterPie.Core.Game.Events;

public class TimeElapsedChangeEventArgs(bool isTimerReset, float timeElapsed) : EventArgs
{

    /// <summary>
    /// Whether the timer has been reset (and switched) into another zero or some non-zero value.
    /// This indicates the time elapsed may experience sudden change.
    /// </summary>
    public bool IsTimerReset { get; } = isTimerReset;

    /// <summary>
    /// Time elapsed in-game
    /// </summary>
    public float TimeElapsed { get; } = timeElapsed;
}