using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Game.Services.Monster;
using HunterPie.Core.Game.Services.Monster.Events;
using HunterPie.DI;
using HunterPie.Playground.Dogma.Configuration;
using HunterPie.Playground.Dogma.ViewModels;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Windows.Threading;

namespace HunterPie.Playground.Dogma.Controller;

internal class DragonsDogmaMonsterWidgetController(
    IContext context,
    ITargetDetectionService targetDetectionService,
    DragonsDogmaMonstersViewModel viewModel,
    DragonsDogmaHealthPluginConfiguration configuration
) : IDisposable
{
    private readonly MonsterWidgetConfiguration config = configuration.MonsterWidget;
    private readonly ConcurrentDictionary<IMonster, DragonsDogmaMonsterController> _monsters = new();

    public DragonsDogmaMonstersViewModel ViewModel => viewModel;

    public void Initialize()
    {
        config.TargetMode.PropertyChanged += OnTargetModeChanged;
        context.Game.OnMonsterSpawn += OnMonsterSpawn;
        context.Game.OnMonsterDespawn += OnMonsterDespawn;
        targetDetectionService.OnTargetChanged += OnInferredTargetChanged;

        Load();
    }

    public void Dispose()
    {
        config.TargetMode.PropertyChanged -= OnTargetModeChanged;
        context.Game.OnMonsterSpawn -= OnMonsterSpawn;
        context.Game.OnMonsterDespawn -= OnMonsterDespawn;
        targetDetectionService.OnTargetChanged -= OnInferredTargetChanged;

        foreach (IMonster monster in _monsters.Keys.ToImmutableList())
            DestroyMonster(monster);
    }

    private void Load()
    {
        foreach (IMonster monster in context.Game.Monsters)
            CreateMonster(monster);
    }

    private void OnTargetChanged(object? sender, MonsterTargetEventArgs e) => UpdateTarget();

    private void OnInferredTargetChanged(object? sender, InferTargetChangedEventArgs e) => UpdateTarget();

    private void OnMonsterSpawn(object? sender, IMonster e) => CreateMonster(e);

    private void OnMonsterDespawn(object? sender, IMonster e) => DestroyMonster(e);

    private void OnTargetModeChanged(object? _, PropertyChangedEventArgs __) => UpdateTarget();

    private void CreateMonster(IMonster monster)
    {
        monster.OnTargetChange += OnTargetChanged;

        var controller = new DragonsDogmaMonsterController(
            dispatcher: DependencyContainer.Get<Dispatcher>(),
            context: monster,
            viewModel: new DragonsDogmaMonsterViewModel()
        );

        if (!_monsters.TryAdd(monster, controller))
            return;

        controller.Initialize();

        viewModel.Target = controller.ViewModel;
    }

    private void DestroyMonster(IMonster monster)
    {
        if (!_monsters.TryRemove(monster, out DragonsDogmaMonsterController? controller))
            return;

        DragonsDogmaMonsterViewModel vm = controller.ViewModel;

        if (viewModel.Target == vm)
            viewModel.Target = null;

        controller.Dispose();
    }

    private void UpdateTarget()
    {
        IMonster? target = config.TargetMode.Value switch
        {
            TargetModeType.LockOn => context.Game.Monsters.FirstOrDefault(
                static (it) => it.Target == Target.Self
            ),
            TargetModeType.MapPin or TargetModeType.AutoQuest => context.Game.Monsters.FirstOrDefault(
                static (it) => it.ManualTarget == Target.Self
            ),
            TargetModeType.Infer => targetDetectionService.Target,
            _ => null
        };

        if (target is not { })
            return;

        if (!_monsters.TryGetValue(target, out DragonsDogmaMonsterController? controller))
            return;

        viewModel.Target = controller.ViewModel;
    }
}