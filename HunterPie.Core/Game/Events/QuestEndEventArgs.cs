using HunterPie.Core.Game.Entity.Game.Quest;
using System;

namespace HunterPie.Core.Game.Events;

public class QuestEndEventArgs : EventArgs
{
    public QuestStatus Status { get; }

    public TimeSpan TimeElapsed { get; }

    public QuestEndEventArgs(QuestStatus status, float timeElapsed)
    {
        Status = status;
        TimeElapsed = TimeSpan.FromSeconds(timeElapsed);
    }
}