using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.System;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using HunterPie.UI.Overlay.Widgets.Monster.Views;
using System;
using System.Linq;
using System.Windows;

namespace HunterPie.UI.Overlay.Widgets.Monster;

public class MonsterWidgetContextHandler : IContextHandler
{
    private readonly MonstersViewModel _viewModel;
    private readonly MonstersView _view;
    private MonsterWidgetConfig Settings => _view.Settings;
    private readonly IContext _context;

    public MonsterWidgetContextHandler(IContext context)
    {
        OverlayConfig config = ClientConfigHelper.GetOverlayConfigFrom(ProcessManager.Game);

        _view = new MonstersView(config.BossesWidget);
        _ = WidgetManager.Register<MonstersView, MonsterWidgetConfig>(_view);

        _viewModel = _view.ViewModel;
        _context = context;

        UpdateData();
        HookEvents();
    }

    private void UpdateData()
    {
        foreach (IMonster monster in _context.Game.Monsters)
        {
            monster.OnTargetChange += OnTargetChange;
            _viewModel.Monsters.Add(new MonsterContextHandler(monster, Settings));
        }

        CalculateVisibleMonsters();
    }

    public void HookEvents()
    {
        _context.Game.OnMonsterSpawn += OnMonsterSpawn;
        _context.Game.OnMonsterDespawn += OnMonsterDespawn;
    }

    public void UnhookEvents()
    {
        _context.Game.OnMonsterSpawn -= OnMonsterSpawn;
        _context.Game.OnMonsterDespawn -= OnMonsterDespawn;
        _ = WidgetManager.Unregister<MonstersView, MonsterWidgetConfig>(_view);
    }

    private void OnMonsterDespawn(object sender, IMonster e)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            MonsterContextHandler monster = _viewModel.Monsters
                .Cast<MonsterContextHandler>()
                .FirstOrDefault(handler => handler.Context == e);

            if (monster is null)
                return;

            _ = _viewModel.Monsters.Remove(monster);
        });

        e.OnTargetChange -= OnTargetChange;
        CalculateVisibleMonsters();
    }

    private void OnMonsterSpawn(object sender, IMonster e)
    {
        _view.Dispatcher.Invoke(() => _viewModel.Monsters.Add(new MonsterContextHandler(e, Settings)));

        e.OnTargetChange += OnTargetChange;
        CalculateVisibleMonsters();
    }

    private void OnTargetChange(object sender, EventArgs e) => CalculateVisibleMonsters();

    private void CalculateVisibleMonsters()
    {
        int targets = _context.Game.Monsters.Count(m => m.IsTarget);

        _viewModel.VisibleMonsters = Settings.ShowOnlyTarget.Value switch
        {
            true => targets,
            false => targets == 0 ? _context.Game.Monsters.Count : targets,
        };
        _viewModel.MonstersCount = _context.Game.Monsters.Count;
    }
}
