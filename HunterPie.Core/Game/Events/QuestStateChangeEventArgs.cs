using HunterPie.Core.Game.Entity.Game;
using HunterPie.Core.Game.Enums;
using System;

namespace HunterPie.Core.Game.Events;
public class QuestStateChangeEventArgs : EventArgs
{
    public bool IsInQuest { get; }
    public TimeSpan QuestTime { get; }
    public QuestStatus Status { get; }

    public QuestStateChangeEventArgs(IGame game)
    {
        IsInQuest = game.IsInQuest;
        QuestTime = TimeSpan.FromSeconds(game.TimeElapsed);
        Status = game.QuestStatus;
    }
}
