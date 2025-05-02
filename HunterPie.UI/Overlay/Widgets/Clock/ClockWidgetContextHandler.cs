using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Game.Quest;
using HunterPie.Core.Game.Events;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Game;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Game.Moon;
using HunterPie.UI.Overlay.Widgets.Clock.ViewModels;
using HunterPie.UI.Overlay.Widgets.Clock.Views;
using System;
using System.Linq;

namespace HunterPie.UI.Overlay.Widgets.Clock;

public class ClockWidgetContextHandler : IContextHandler
{
    private readonly ClockViewModel _viewModel;
    private readonly ClockView _view;
    private readonly IContext _context;

    public ClockWidgetContextHandler(
        IContext context,
        ClockWidgetConfig configuration
    )
    {
        _view = new ClockView(configuration);
        _viewModel = _view.ViewModel;
        _context = context;

        WidgetManager.Register<ClockView, ClockWidgetConfig>(
            widget: _view
        );

        HookEvents();
        UpdateData();
    }

    public void HookEvents()
    {
        _context.Game.OnWorldTimeChange += OnWorldTimeChange;
        _context.Game.OnQuestStart += OnQuestStart;
        _context.Game.OnQuestEnd += OnQuestEnd;

        if (_context.Game.Quest is not { } quest)
            return;

        quest.OnTimeLeftChange += OnQuestTimeLeftChange;
    }

    public void UnhookEvents()
    {
        WidgetManager.Unregister<ClockView, ClockWidgetConfig>(
            widget: _view
        );

        _context.Game.OnQuestStart -= OnQuestStart;
        _context.Game.OnQuestEnd -= OnQuestEnd;
        _context.Game.OnWorldTimeChange -= OnWorldTimeChange;

        if (_context.Game is MHWildsGame game)
            game.MoonChanged -= OnMoonChanged;

        if (_context.Game.Quest is not { } quest)
            return;

        quest.OnTimeLeftChange -= OnQuestTimeLeftChange;
    }

    private void SetupMoons()
    {
        if (_context.Game is not MHWildsGame game)
            return;

        MoonViewModel[] moonViewModels = Enum.GetValues<MoonPhase>()
            .Select(it => new MoonViewModel { IsActive = false, Phase = it, })
            .ToArray();

        foreach (MoonViewModel model in moonViewModels)
            _viewModel.Moons.Add(model);

        _viewModel.Moon = moonViewModels.FirstOrDefault(it => it.Phase == game.Moon);

        if (_viewModel.Moon is { })
            _viewModel.Moon.IsActive = true;

        game.MoonChanged += OnMoonChanged;
    }

    private void UpdateData()
    {
        _viewModel.WorldTime = _context.Game.WorldTime;
        _viewModel.QuestTimeLeft = _context.Game.Quest?.TimeLeft;

        SetupMoons();
    }

    private void OnQuestEnd(object sender, QuestEndEventArgs e)
    {
        e.Quest.OnTimeLeftChange -= OnQuestTimeLeftChange;

        _viewModel.QuestTimeLeft = null;
    }

    private void OnQuestStart(object sender, IQuest e)
    {
        _viewModel.QuestTimeLeft = e.TimeLeft;

        e.OnTimeLeftChange += OnQuestTimeLeftChange;
    }

    private void OnWorldTimeChange(object sender, SimpleValueChangeEventArgs<TimeOnly> e)
    {
        _viewModel.WorldTime = e.NewValue;
    }

    private void OnQuestTimeLeftChange(object sender, SimpleValueChangeEventArgs<TimeSpan> e)
    {
        _viewModel.QuestTimeLeft = e.NewValue;
    }

    private void OnMoonChanged(object sender, SimpleValueChangeEventArgs<MoonPhase> e)
    {
        if (_viewModel.Moon is not null)
            _viewModel.Moon.IsActive = false;

        _viewModel.Moon = _viewModel.Moons.FirstOrDefault(it => it.Phase == e.NewValue);

        if (_viewModel.Moon is { })
            _viewModel.Moon.IsActive = true;
    }
}