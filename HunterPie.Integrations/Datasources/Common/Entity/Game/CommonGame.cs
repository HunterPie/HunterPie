using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Entity.Game;
using HunterPie.Core.Game.Entity.Game.Chat;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Game.Services;

namespace HunterPie.Integrations.Datasources.Common.Entity.Game;
public abstract class CommonGame : Scannable, IGame, IEventDispatcher
{

    public abstract IPlayer Player { get; }
    public abstract IAbnormalityCategorizationService AbnormalityCategorizationService { get; }
    public abstract List<IMonster> Monsters { get; }
    public abstract IChat? Chat { get; }
    public abstract bool IsHudOpen { get; protected set; }
    public abstract float TimeElapsed { get; protected set; }
    public abstract int MaxDeaths { get; protected set; }
    public abstract int Deaths { get; protected set; }

    protected readonly SmartEvent<IMonster> _onMonsterSpawn = new();
    public event EventHandler<IMonster> OnMonsterSpawn
    {
        add => _onMonsterSpawn.Hook(value);
        remove => _onMonsterSpawn.Unhook(value);
    }

    protected readonly SmartEvent<IMonster> _onMonsterDespawn = new();
    public event EventHandler<IMonster> OnMonsterDespawn
    {
        add => _onMonsterDespawn.Hook(value);
        remove => _onMonsterDespawn.Unhook(value);
    }

    protected readonly SmartEvent<IGame> _onHudStateChange = new();
    public event EventHandler<IGame> OnHudStateChange
    {
        add => _onHudStateChange.Hook(value);
        remove => _onHudStateChange.Unhook(value);
    }

    protected readonly SmartEvent<TimeElapsedChangeEventArgs> _onTimeElapsedChange = new();
    public event EventHandler<TimeElapsedChangeEventArgs> OnTimeElapsedChange
    {
        add => _onTimeElapsedChange.Hook(value);
        remove => _onTimeElapsedChange.Unhook(value);
    }

    protected readonly SmartEvent<IGame> _onDeathCountChange = new();
    public event EventHandler<IGame> OnDeathCountChange
    {
        add => _onDeathCountChange.Hook(value);
        remove => _onDeathCountChange.Unhook(value);
    }

    protected CommonGame(IProcessManager process) : base(process) { }

    public virtual void Dispose()
    {
        IDisposable[] events =
        {
            _onMonsterSpawn, _onMonsterDespawn, _onHudStateChange, _onTimeElapsedChange, _onDeathCountChange
        };

        Monsters.TryCast<IDisposable>()
                .DisposeAll();

        if (Player is IDisposable player)
            player.Dispose();

        if (Chat is IDisposable chat)
            chat.Dispose();

        events.DisposeAll();
    }
}
