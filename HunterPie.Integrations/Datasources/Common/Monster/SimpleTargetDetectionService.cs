using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Entity.Game;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Game.Services.Monster;
using HunterPie.Core.Game.Services.Monster.Events;
using System.Collections.Concurrent;
using System.Numerics;
using TargetType = HunterPie.Core.Game.Enums.Target;

namespace HunterPie.Integrations.Datasources.Common.Monster;

internal class SimpleTargetDetectionService : ITargetDetectionService, IDisposable, IEventDispatcher
{
    // 6400 is 80 meters
    private const double MAXIMUM_DISTANCE = 6400.0;
    private readonly IGame _game;
    private readonly IPlayer _player;

    private readonly Lock _lock = new();
    private readonly ConcurrentDictionary<IMonster, double> _monsterDistances = new();

    public IMonster? Target
    {
        get
        {
            lock (_lock)
                return field;
        }
        private set
        {
            lock (_lock)
            {
                if (field == value)
                    return;

                field = value;
            }

            this.Dispatch(
                toDispatch: _onTargetChanged,
                data: new InferTargetChangedEventArgs(value)
            );
        }
    }

    public SimpleTargetDetectionService(IContext context)
    {
        _game = context.Game;
        _player = context.Game.Player;
        HookEvents();
    }

    private readonly SmartEvent<InferTargetChangedEventArgs> _onTargetChanged = new();
    public event EventHandler<InferTargetChangedEventArgs> OnTargetChanged
    {
        add => _onTargetChanged.Hook(value);
        remove => _onTargetChanged.Unhook(value);
    }

    public TargetType Infer(IMonster monster)
    {
        if (Target is not { } cached)
            return TargetType.None;

        return cached == monster
            ? TargetType.Self
            : TargetType.Another;
    }

    private void HookEvents()
    {
        _game.OnMonsterSpawn += OnMonsterSpawn;
        _game.OnMonsterDespawn += OnMonsterDespawn;
        _player.PositionChange += OnPlayerPositionChange;
    }

    private void UnhookEvents()
    {
        _game.OnMonsterSpawn -= OnMonsterSpawn;
        _game.OnMonsterDespawn -= OnMonsterDespawn;
        _player.PositionChange -= OnPlayerPositionChange;
    }

    private void OnMonsterSpawn(object? sender, IMonster e)
    {
        Target ??= e;

        e.OnHealthChange += OnMonsterHealthChange;
        e.PositionChange += OnMonsterPositionChange;
    }

    private void OnMonsterDespawn(object? sender, IMonster e)
    {
        Target = _game.Monsters.SingleOrNull();

        e.OnHealthChange -= OnMonsterHealthChange;
        e.PositionChange -= OnMonsterPositionChange;
    }

    private void OnMonsterHealthChange(object? sender, EventArgs e)
    {
        if (sender is not IMonster monster)
            return;

        InferNextTarget();
    }

    private void OnMonsterPositionChange(object? sender, SimpleValueChangeEventArgs<Vector3> e)
    {
        if (sender is not IMonster monster)
            return;

        HandlePositionChange(_player, monster);

        InferNextTarget();
    }


    private void OnPlayerPositionChange(object? sender, SimpleValueChangeEventArgs<Vector3> e)
    {
        if (sender is not IPlayer player)
            return;

        foreach (IMonster monster in _game.Monsters)
            HandlePositionChange(player, monster);

        InferNextTarget();
    }

    private void HandlePositionChange(IPlayer player, IMonster monster)
    {
        float distance = Vector3.DistanceSquared(player.Position, monster.Position);

        _monsterDistances.AddOrUpdate(monster, distance, (_, __) => distance);
    }

    private void InferNextTarget()
    {
        IMonster? nearestMonster = _monsterDistances
            .Where(it => it.Value <= MAXIMUM_DISTANCE)
            .OrderBy(it =>
            {
                double distance = it.Value;
                double damageTaken = it.Key.Health / Math.Max(it.Key.MaxHealth, 1.0);

                return damageTaken / distance;
            })
            .FirstOrDefault()
            .Key;

        Target = nearestMonster;
    }

    public void Dispose()
    {
        _onTargetChanged.Dispose();
        UnhookEvents();
    }
}