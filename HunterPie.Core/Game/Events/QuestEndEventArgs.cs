using HunterPie.Core.Game.Entity.Game.Quest;
using System;

namespace HunterPie.Core.Game.Events;

public class QuestEndEventArgs(IQuest quest, QuestStatus status, float timeElapsed) : EventArgs
{
    public IQuest Quest { get; } = quest;

    public QuestStatus Status { get; } = status;

    public TimeSpan TimeElapsed { get; } = TimeSpan.FromSeconds(timeElapsed);
}