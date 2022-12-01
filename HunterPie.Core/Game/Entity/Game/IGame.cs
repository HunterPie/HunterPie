using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Entity.Game.Chat;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Game.Services;
using System;
using System.Collections.Generic;

namespace HunterPie.Core.Game.Entity.Game;

#nullable enable
public interface IGame : IDisposable
{

    public IPlayer Player { get; }
    public IAbnormalityCategorizationService AbnormalityCategorizationService { get; }
    public List<IMonster> Monsters { get; }

    public IChat? Chat { get; }

    public bool IsHudOpen { get; }

    public float TimeElapsed { get; }

    public int MaxDeaths { get; }

    public int Deaths { get; }

    public event EventHandler<IMonster> OnMonsterSpawn;
    public event EventHandler<IMonster> OnMonsterDespawn;
    public event EventHandler<IGame> OnHudStateChange;
    public event EventHandler<TimeElapsedChangeEventArgs> OnTimeElapsedChange;
    public event EventHandler<IGame> OnDeathCountChange;
}
