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
using HunterPie.Core.Observability.Logging;
using System.Collections.Concurrent;
using System.Numerics;
using TargetType = HunterPie.Core.Game.Enums.Target;

namespace HunterPie.Integrations.Datasources.Common.Monster;

internal class SimpleTargetDetectionService : ITargetDetectionService, IDisposable, IEventDispatcher
{
    private readonly ILogger _logger = LoggerFactory.Create();
    private const double MAXIMUM_DISTANCE_METERS = 80.0;
    private const double RECENT_HIT_DURATION_SECONDS = 30.0;
    private readonly IGame _game;
    private readonly IPlayer _player;

    private readonly Lock _lock = new();
    private readonly ConcurrentDictionary<IMonster, TargetInferenceParams> _inferenceParams = new();

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

            _logger.Debug($"New target set to {value?.Name}");
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

        _inferenceParams.AddOrUpdate(
            key: monster,
            addValueFactory: static (m) => new TargetInferenceParams(
                LastHitAt: DateTime.Now,
                HealthRatio: m.Health / Math.Max(m.MaxHealth, 1.0),
                Distance: MAXIMUM_DISTANCE_METERS
            ),
            updateValueFactory: static (m, existing) => existing with
            {
                LastHitAt = DateTime.Now,
                HealthRatio = m.Health / Math.Max(m.MaxHealth, 1.0),
            }
        );
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
        float distance = Vector3.Distance(player.Position, monster.Position);

        _inferenceParams.AddOrUpdate(
            key: monster,
            addValueFactory: (m) => new TargetInferenceParams(
                LastHitAt: DateTime.Now,
                HealthRatio: m.Health / Math.Max(m.MaxHealth, 1.0),
                Distance: distance
            ),
            updateValueFactory: (m, existing) => existing with
            {
                Distance = distance
            }
        );
    }

    private void InferNextTarget()
    {
        IMonster? nearestMonster = _inferenceParams
            .Where(it => it.Value.Distance <= MAXIMUM_DISTANCE_METERS)
            .OrderByDescending(it =>
            {
                TargetInferenceParams inferenceParams = it.Value;
                double normalizedDistance = 1 - (inferenceParams.Distance / MAXIMUM_DISTANCE_METERS);
                double normalizedDamageTime = Math.Max(0, 1 - ((DateTime.Now - inferenceParams.LastHitAt).TotalSeconds / RECENT_HIT_DURATION_SECONDS));
                double normalizedHealthRatio = 1 - inferenceParams.HealthRatio;

                double score =
                    (normalizedDistance * 0.45) +
                    (normalizedDamageTime * 0.40) +
                    (normalizedHealthRatio * 0.15);

                if (inferenceParams.HealthRatio >= 0.99)
                    score *= 0.5;

                return score;
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