using HunterPie.Core.Game.Entity.Player;
using System;

namespace HunterPie.Core.Game.Events;

public class LevelChangeEventArgs(IPlayer player) : EventArgs
{
    public int HighRank { get; } = player.HighRank;
    public int MasterRank { get; } = player.MasterRank;
}