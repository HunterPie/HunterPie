using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using HunterPie.UI.Overlay.Widgets.Monster.Adapters;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using System;
using System.ComponentModel;
using System.Linq;

namespace HunterPie.UI.Overlay.Widgets.Monster;

public class MonsterWidgetContextHandler : IContextHandler
{
    private readonly MonstersViewModel _viewModel;
    private readonly IContext _context;
    private readonly MonsterWidgetConfig _config;

    public MonsterWidgetContextHandler(
        IContext context,
        MonstersViewModel viewModel,
        MonsterWidgetConfig config)
    {
        _config = config;
        _viewModel = viewModel;
        _context = context;

        HookEvents();
        UpdateData();
    }

    private void UpdateData()
    {
        foreach (IMonster monster in _context.Game.Monsters)
        {
            monster.OnTargetChange += OnTargetChange;
            _viewModel.Monsters.Add(new MonsterContextHandler(_context.Game, monster, _config));
        }

        CalculateVisibleMonsters();
    }

    public void HookEvents()
    {
        _config.TargetMode.PropertyChanged += OnTargetModeChanged;
        _config.IsTargetingEnabled.PropertyChanged += OnTargetModeChanged;
        _context.Game.OnMonsterSpawn += OnMonsterSpawn;
        _context.Game.OnMonsterDespawn += OnMonsterDespawn;
    }

    public void UnhookEvents()
    {
        _config.TargetMode.PropertyChanged -= OnTargetModeChanged;
        _config.IsTargetingEnabled.PropertyChanged -= OnTargetModeChanged;
        _context.Game.OnMonsterSpawn -= OnMonsterSpawn;
        _context.Game.OnMonsterDespawn -= OnMonsterDespawn;

        _viewModel.UIThread.BeginInvoke(() =>
        {
            foreach (MonsterContextHandler ctxHandler in _viewModel.Monsters.Cast<MonsterContextHandler>())
                ctxHandler.Dispose();

            _viewModel.Monsters.Clear();
        });
    }

    private void OnTargetModeChanged(object _, PropertyChangedEventArgs __) => CalculateVisibleMonsters();

    private void OnMonsterDespawn(object sender, IMonster e)
    {
        _viewModel.UIThread.BeginInvoke(() =>
        {
            MonsterContextHandler monster = _viewModel.Monsters
                .Cast<MonsterContextHandler>()
                .FirstOrDefault(handler => handler.Context == e);

            if (monster is null)
                return;

            monster.Dispose();

            _ = _viewModel.Monsters.Remove(monster);

            if (_viewModel.Monster == monster)
                _viewModel.Monster = null;
        });

        e.OnTargetChange -= OnTargetChange;
        CalculateVisibleMonsters();
    }

    private void OnMonsterSpawn(object sender, IMonster monster)
    {
        _viewModel.UIThread.BeginInvoke(() =>
        {
            _viewModel.Monsters.Add(new MonsterContextHandler(_context.Game, monster, _config));

            monster.OnTargetChange += OnTargetChange;
            CalculateVisibleMonsters();
        });
    }

    private void OnTargetChange(object sender, EventArgs e) => CalculateVisibleMonsters();

    private void CalculateVisibleMonsters()
    {
        MonsterContextHandler[] targets = _viewModel.Monsters.Cast<MonsterContextHandler>()
            .Where(it =>
            {
                Target target = MonsterTargetAdapter.Adapt(
                    config: _config,
                    lockOnTarget: it.Context.Target,
                    manualTarget: it.Context.ManualTarget,
                    inferredTarget: _context.Game.TargetDetectionService.Infer(it.Context)
                );

                return it.Context.Health > 0 && target == Target.Self;
            }).ToArray();

        _viewModel.Monster = targets.SingleOrNull();
        _viewModel.VisibleMonsters = _config.ShowOnlyTarget.Value switch
        {
            true => targets.Length,
            false => targets.Length == 0 ? _context.Game.Monsters.Count : targets.Length,
        };
        _viewModel.MonstersCount = _context.Game.Monsters.Count;
    }
}