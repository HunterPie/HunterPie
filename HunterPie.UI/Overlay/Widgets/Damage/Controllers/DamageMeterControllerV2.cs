using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Game.Quest;
using HunterPie.Core.Game.Entity.Party;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Observability.Logging;
using HunterPie.UI.Architecture.Brushes;
using HunterPie.UI.Overlay.Widgets.Damage.Helpers;
using HunterPie.UI.Overlay.Widgets.Damage.View;
using HunterPie.UI.Overlay.Widgets.Damage.ViewModels;
using HunterPie.UI.Settings.Converter;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using Color = System.Windows.Media.Color;
using ObservableColor = HunterPie.Core.Settings.Types.Color;
using Range = HunterPie.Core.Settings.Types.Range;


namespace HunterPie.UI.Overlay.Widgets.Damage.Controllers;

#nullable enable
public class DamageMeterControllerV2 : IContextHandler
{
    private readonly ILogger _logger = LoggerFactory.Create();

    private readonly IContext _context;
    private MeterViewModel _viewModel;
    private MeterViewModel _viewModelSnapshot;
    private readonly MeterView _view;
    private readonly DamageMeterWidgetConfig _config;
    private readonly Dictionary<IPartyMember, PartyMemberContext> _members = new();

    public DamageMeterControllerV2(
        IContext context,
        MeterView view,
        DamageMeterWidgetConfig config)
    {
        _context = context;
        _view = view;
        _config = config;
        _viewModel = _view.ViewModel;
        _viewModelSnapshot = _view.ViewModel;

        WidgetManager.Register<MeterView, DamageMeterWidgetConfig>(
            widget: _view
        );

        HookEvents();
        UpdateData();
    }

    public void HookEvents()
    {
        _context.Game.Player.Party.OnMemberJoin += OnMemberJoin;
        _context.Game.Player.Party.OnMemberLeave += OnMemberLeave;
        _context.Game.OnTimeElapsedChange += OnTimeElapsedChange;
        _context.Game.Player.OnStageUpdate += OnStageUpdate;
        _context.Game.OnQuestStart += OnQuestStart;
        _context.Game.OnQuestEnd += OnQuestEnd;

        if (_context.Game.Quest is { } quest)
            quest.OnDeathCounterChange += OnDeathCounterChange;
    }

    public void UnhookEvents()
    {
        _context.Game.Player.Party.OnMemberJoin -= OnMemberJoin;
        _context.Game.Player.Party.OnMemberLeave -= OnMemberLeave;
        _context.Game.OnTimeElapsedChange -= OnTimeElapsedChange;
        _context.Game.Player.OnStageUpdate -= OnStageUpdate;
        _context.Game.OnQuestStart -= OnQuestStart;
        _context.Game.OnQuestEnd -= OnQuestEnd;

        foreach (IPartyMember member in _members.Keys)
            HandleMemberLeave(member);

        if (_context.Game.Quest is { } quest)
            quest.OnDeathCounterChange -= OnDeathCounterChange;

        WidgetManager.Unregister<MeterView, DamageMeterWidgetConfig>(
            widget: _view
        );
    }

    private void OnDeathCounterChange(object? sender, CounterChangeEventArgs e)
    {
        _viewModel.MaxDeaths = e.Max;
        _viewModel.Deaths = e.Current;
    }

    private void OnQuestEnd(object? sender, QuestEndEventArgs e)
    {
        _view.Dispatcher.BeginInvoke(() =>
        {
            foreach (IPartyMember member in _members.Keys)
            {
                member.OnDamageDealt -= OnDamageDealt;
                member.OnWeaponChange -= OnWeaponChange;
            }

            _viewModel.TimeElapsed = e.TimeElapsed.TotalSeconds;
            _viewModelSnapshot = _viewModel;
            _view.DataContext = _viewModelSnapshot;

            _viewModel = new MeterViewModel(_config);
        });
    }

    private void OnQuestStart(object? sender, IQuest e)
    {
        _view.Dispatcher.BeginInvoke(() =>
        {
            e.OnDeathCounterChange += OnDeathCounterChange;
            _viewModel.MaxDeaths = e.MaxDeaths;
            _viewModel.Deaths = e.Deaths;

            _view.DataContext = _viewModel;
            _viewModel.InHuntingZone = true;
        });
    }

    private void OnStageUpdate(object? sender, EventArgs e)
    {
        bool isVisibleArea = _context.Game.Player.InHuntingZone || _context.Game.Quest is not null;
        _viewModelSnapshot.InHuntingZone = isVisibleArea;
        _viewModel.InHuntingZone = isVisibleArea;
    }

    private void OnTimeElapsedChange(object? sender, TimeElapsedChangeEventArgs e)
    {
        _viewModel.TimeElapsed = e.TimeElapsed;

        _view.Dispatcher.BeginInvoke(CalculatePlayerSeries);
    }

    private void OnMemberLeave(object? sender, IPartyMember e) =>
        _view.Dispatcher.BeginInvoke(() => HandleMemberLeave(e));

    private void OnMemberJoin(object? sender, IPartyMember e) =>
        _view.Dispatcher.BeginInvoke(() => HandleMemberJoin(e));

    private void UpdateData()
    {
        _viewModel.InHuntingZone = _context.Game.Player.InHuntingZone || _context.Game.Quest is not null;
        _viewModel.MaxDeaths = _context.Game.Quest?.MaxDeaths ?? 0;
        _viewModel.Deaths = _context.Game.Quest?.Deaths ?? 0;
        _viewModel.TimeElapsed = _context.Game.TimeElapsed;

        foreach (IPartyMember member in _context.Game.Player.Party.Members)
            HandleMemberJoin(member);
    }

    private void OnWeaponChange(object? sender, IPartyMember e)
    {
        if (!_members.TryGetValue(e, out PartyMemberContext? memberCtx))
            return;

        memberCtx.ViewModel.Weapon = e.Weapon;
    }

    private void OnDamageDealt(object? sender, IPartyMember e)
    {
        if (!_members.TryGetValue(e, out PartyMemberContext? memberCtx))
            return;

        if (memberCtx.ViewModel.Damage <= 0)
            memberCtx.FirstHitAt = _context.Game.TimeElapsed;

        double newDps = CalculateDpsByConfiguredStrategy(memberCtx);
        memberCtx.ViewModel.IsIncreasing = newDps > memberCtx.ViewModel.DPS;
        memberCtx.ViewModel.Damage = e.Damage - memberCtx.IgnorableDamage;
        memberCtx.ViewModel.DPS = newDps;
    }

    private void HandleMemberJoin(IPartyMember member)
    {
        if (_members.ContainsKey(member))
            return;

        ObservableColor playerColor = PlayerConfigHelper.GetColorFromPlayer(
            game: _context.Process.Type,
            slot: member.Slot,
            isSelf: member.IsMyself
        );

        var memberContext = new PartyMemberContext
        {
            ViewModel = new PlayerViewModel(_config)
            {
                Name = member.Name,
                Damage = member.Damage,
                Weapon = member.Weapon,
                Bar = new DamageBarViewModel(playerColor),
                IsUser = member.IsMyself,
                MasterRank = member.MasterRank,
            },
            Plots = BuildPlayerPlots(
                name: member.Name,
                color: playerColor
            ),
            JoinedAt = _context.Game.TimeElapsed,
            FirstHitAt = member.Damage > 0
                ? _context.Game.TimeElapsed
                : -1,
            IgnorableDamage = 0
        };

        _members.Add(member, memberContext);
        _viewModel.Players.Add(memberContext.ViewModel);

        member.OnDamageDealt += OnDamageDealt;
        member.OnWeaponChange += OnWeaponChange;

        _logger.Debug($"added player {member.Name} to party [{member.GetHashCode():08X}]");
    }

    private void HandleMemberLeave(IPartyMember member)
    {
        member.OnDamageDealt -= OnDamageDealt;
        member.OnWeaponChange -= OnWeaponChange;

        if (!_members.Remove(member, out PartyMemberContext? memberCtx))
            return;

        _viewModel.Players.Remove(memberCtx.ViewModel);
        _viewModel.Series.Remove(memberCtx.Plots);

        _logger.Debug($"removed player {member.Name} from party [{member.GetHashCode():08X}]");
    }

    private void CalculatePlayerSeries()
    {
        float totalDamage = _members.Keys.Sum(it => it.Damage);

        foreach ((IPartyMember member, PartyMemberContext memberCtx) in _members)
        {
            if (memberCtx.Plots.Values is not ChartValues<ObservablePoint> chartValues)
                continue;

            memberCtx.ViewModel.Bar.Percentage = totalDamage > 0
                ? member.Damage / totalDamage * 100
                : 0;

            double newDps = CalculateDpsByConfiguredStrategy(memberCtx);
            memberCtx.ViewModel.IsIncreasing = newDps > memberCtx.ViewModel.DPS;
            memberCtx.ViewModel.DPS = newDps;

            ObservablePoint point = CalculatePointByConfiguredStrategy(memberCtx.ViewModel);

            chartValues.Add(point);
        }

        _viewModel.SortMembers();
    }

    private double CalculateDpsByConfiguredStrategy(PartyMemberContext memberContext)
    {
        double timeElapsed = _config.DpsCalculationStrategy.Value switch
        {
            DPSCalculationStrategy.RelativeToQuest => _viewModel.TimeElapsed,
            DPSCalculationStrategy.RelativeToJoin => _viewModel.TimeElapsed - Math.Min(_viewModel.TimeElapsed, memberContext.JoinedAt),
            DPSCalculationStrategy.RelativeToFirstHit => _viewModel.TimeElapsed - Math.Min(_viewModel.TimeElapsed, memberContext.FirstHitAt),
            _ => 1,
        };
        timeElapsed = Math.Max(1, timeElapsed);

        return memberContext.ViewModel.Damage / timeElapsed;
    }

    private ObservablePoint CalculatePointByConfiguredStrategy(PlayerViewModel player)
    {
        double damage = _config.DamagePlotStrategy.Value switch
        {
            DamagePlotStrategy.TotalDamage => player.Damage,
            DamagePlotStrategy.DamagePerSecond => player.DPS,
            _ => throw new NotImplementedException(),
        };

        return new ObservablePoint(_viewModel.TimeElapsed, damage);
    }

    private Series BuildPlayerPlots(string name, string color)
    {
        ChartValues<ObservablePoint> points = new();

        var actualColor = (Color)ColorConverter.ConvertFromString(color);
        var series = new LineSeries
        {
            Title = name,
            Stroke = new SolidColorBrush(actualColor),
            Fill = ColorFadeGradient.FromColor(actualColor),
            PointGeometry = null,
            Values = points
        };
        Binding smoothingBinding = VisualConverterHelper.CreateBinding(_viewModel.Settings.PlotLineSmoothing, nameof(Range.Current));
        Binding thicknessBinding =
            VisualConverterHelper.CreateBinding(_viewModel.Settings.PlotLineThickness, nameof(Range.Current));

        series.SetBinding(Series.StrokeThicknessProperty, thicknessBinding);
        series.SetBinding(LineSeries.LineSmoothnessProperty, smoothingBinding);

        _viewModel.Series.Add(series);
        return series;
    }
}