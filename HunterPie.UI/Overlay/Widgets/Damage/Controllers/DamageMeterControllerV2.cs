﻿using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Game.Quest;
using HunterPie.Core.Game.Entity.Party;
using HunterPie.Core.Game.Events;
using HunterPie.Core.List;
using HunterPie.Core.Observability.Logging;
using HunterPie.UI.Architecture.Brushes;
using HunterPie.UI.Architecture.Colors;
using HunterPie.UI.Overlay.Widgets.Damage.Helpers;
using HunterPie.UI.Overlay.Widgets.Damage.View;
using HunterPie.UI.Overlay.Widgets.Damage.ViewModels;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using Color = System.Windows.Media.Color;
using ObservableColor = HunterPie.Core.Settings.Types.Color;


namespace HunterPie.UI.Overlay.Widgets.Damage.Controllers;

#nullable enable
public class DamageMeterControllerV2 : IContextHandler
{
    private readonly ILogger _logger = LoggerFactory.Create();

    private readonly IContext _context;
    private MeterViewModel _viewModel;
    private MeterViewModel _viewModelSnapshot;
    private readonly MeterViewV2 _view;
    private readonly DamageMeterWidgetConfig _config;
    private readonly ConcurrentDictionary<IPartyMember, PartyMemberContext> _members = new();
    private int _windowSecondsCount;

    public DamageMeterControllerV2(
        IContext context,
        MeterViewV2 view,
        DamageMeterWidgetConfig config)
    {
        _context = context;
        _view = view;
        _config = config;
        _viewModel = _view.ViewModel;
        _viewModelSnapshot = _view.ViewModel;

        WidgetManager.Register<MeterViewV2, DamageMeterWidgetConfig>(
            widget: _view
        );

        HookEvents();
        UpdateData();
    }

    public void HookEvents()
    {
        _context.Game.Player.Party.OnMemberJoin += OnMemberJoin;
        _context.Game.Player.Party.OnMemberLeave += OnMemberLeave;
        _context.Game.Player.OnVillageEnter += OnVillageStateUpdate;
        _context.Game.Player.OnVillageLeave += OnVillageStateUpdate;
        _context.Game.Player.OnStageUpdate += OnStageUpdate;
        _context.Game.OnTimeElapsedChange += OnTimeElapsedChange;
        _context.Game.OnQuestStart += OnQuestStart;
        _context.Game.OnQuestEnd += OnQuestEnd;
        _config.PlotSamplingInSeconds.PropertyChanged += OnPlotSlidingWindowChange;

        if (_context.Game.Quest is { } quest)
            quest.OnDeathCounterChange += OnDeathCounterChange;
    }

    public void UnhookEvents()
    {
        _context.Game.Player.Party.OnMemberJoin -= OnMemberJoin;
        _context.Game.Player.Party.OnMemberLeave -= OnMemberLeave;
        _context.Game.Player.OnVillageEnter -= OnVillageStateUpdate;
        _context.Game.Player.OnVillageLeave -= OnVillageStateUpdate;
        _context.Game.Player.OnStageUpdate -= OnStageUpdate;
        _context.Game.OnTimeElapsedChange -= OnTimeElapsedChange;
        _context.Game.OnQuestStart -= OnQuestStart;
        _context.Game.OnQuestEnd -= OnQuestEnd;
        _config.PlotSamplingInSeconds.PropertyChanged -= OnPlotSlidingWindowChange;

        foreach (IPartyMember member in _members.Keys)
            HandleMemberLeave(member);

        if (_context.Game.Quest is { } quest)
            quest.OnDeathCounterChange -= OnDeathCounterChange;

        WidgetManager.Unregister<MeterViewV2, DamageMeterWidgetConfig>(
            widget: _view
        );
    }

    private void OnPlotSlidingWindowChange(object? sender, PropertyChangedEventArgs e)
    {
        _members.Values.ForEach(it => it.DamageHistory.AdjustSize((int)_config.PlotSamplingInSeconds.Current));
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

            _members.Clear();

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
            _members.Clear();
            _viewModel.Players.Clear();
            _viewModel.Series.Clear();

            foreach (IPartyMember member in _context.Game.Player.Party.Members)
                HandleMemberJoin(member);

            e.OnDeathCounterChange += OnDeathCounterChange;
            _viewModel.MaxDeaths = e.MaxDeaths;
            _viewModel.Deaths = e.Deaths;

            _view.DataContext = _viewModel;
            _viewModel.InHuntingZone = true;
        });
    }

    private void OnStageUpdate(object? sender, EventArgs e)
    {
        if (_context.Game.Quest is not null)
            return;

        _view.Dispatcher.BeginInvoke(() =>
        {
            _members.Clear();
            _viewModel.Players.Clear();
            _viewModel.Series.Clear();

            foreach (IPartyMember member in _context.Game.Player.Party.Members)
                HandleMemberJoin(member);

            _viewModel.MaxDeaths = 0;

            _view.DataContext = _viewModel;
            _viewModel.InHuntingZone = _context.Game.Player.InHuntingZone;
        });
    }


    private void OnVillageStateUpdate(object? sender, EventArgs e)
    {
        bool isVisibleArea = _context.Game.Player.InHuntingZone || _context.Game.Quest is not null;
        _viewModelSnapshot.InHuntingZone = isVisibleArea;
        _viewModel.InHuntingZone = isVisibleArea;
    }

    private void OnTimeElapsedChange(object? sender, TimeElapsedChangeEventArgs e)
    {
        const double precision = 0.5;
        double lastUpdate = _viewModel.TimeElapsed % precision;
        double newUpdate = e.TimeElapsed % precision;

        _viewModel.TimeElapsed = e.TimeElapsed;

        bool canUpdate = newUpdate < lastUpdate;

        if (!canUpdate)
            return;

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
        if (e.Damage <= 0)
            return;

        if (!_members.TryGetValue(e, out PartyMemberContext? memberCtx))
            return;

        if (memberCtx.ViewModel.Damage <= 0)
            memberCtx.FirstHitAt = _context.Game.TimeElapsed;

        memberCtx.ViewModel.Damage = e.Damage;
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
            DamageHistory = new SlidingWindow<int>((int)_config.PlotSamplingInSeconds.Current),
        };

        if (!_members.TryAdd(member, memberContext))
            return;

        _viewModel.Players.Add(memberContext.ViewModel);

        member.OnDamageDealt += OnDamageDealt;
        member.OnWeaponChange += OnWeaponChange;

        _logger.Debug($"added player {member.Name} | {memberContext.JoinedAt} to party [{member.GetHashCode():X08}]");
    }

    private void HandleMemberLeave(IPartyMember member)
    {
        member.OnDamageDealt -= OnDamageDealt;
        member.OnWeaponChange -= OnWeaponChange;

        if (!_members.Remove(member, out PartyMemberContext? memberCtx))
            return;

        _viewModel.Players.Remove(memberCtx.ViewModel);
        _viewModel.Series.Remove(memberCtx.Plots);

        _logger.Debug($"removed player {member.Name} from party [{member.GetHashCode():X08}]");
    }

    private void CalculatePlayerSeries()
    {
        float totalDamage = _members.Keys.Sum(it => it.Damage);
        bool shouldDiscardOldPlots = _config.IsOldPlotDiscardingEnabled;
        double plottingWindowSize = _config.PlotSlidingWindowInSeconds.Current;
        double plotSampleRate = _config.PlotSamplingInSeconds.Current;
        int secondsCount = (int)Math.Floor(_viewModel.TimeElapsed % plotSampleRate);
        bool hasBeenOneSecond = secondsCount != _windowSecondsCount;
        _windowSecondsCount = secondsCount;

        foreach ((IPartyMember member, PartyMemberContext memberCtx) in _members)
        {
            if (memberCtx.Plots.Values is not ChartValues<ObservablePoint> chartValues)
                continue;

            memberCtx.ViewModel.Bar.Percentage = totalDamage > 0
                ? member.Damage / totalDamage * 100
                : 0;

            if (hasBeenOneSecond)
                memberCtx.DamageHistory.Add(memberCtx.ViewModel.Damage);

            double newDps = CalculateDpsByConfiguredStrategy(memberCtx);
            memberCtx.ViewModel.IsIncreasing = newDps > memberCtx.ViewModel.DPS;
            memberCtx.ViewModel.DPS = newDps;

            ObservablePoint point = CalculatePointByConfiguredStrategy(memberCtx, plotSampleRate);
            chartValues.Add(point);

            if (shouldDiscardOldPlots)
                ClearOldPoints(chartValues, plottingWindowSize);
        }

        _viewModel.SortMembers();
    }

    private void ClearOldPoints(ChartValues<ObservablePoint> chart, double plottingWindowSize)
    {
        bool isAboveConfiguredWindowSize;

        do
        {
            ObservablePoint point = chart[0];
            isAboveConfiguredWindowSize = (_viewModel.TimeElapsed - point.X) > plottingWindowSize;

            if (!isAboveConfiguredWindowSize)
                break;

            chart.RemoveAt(0);
        } while (isAboveConfiguredWindowSize);
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

    private ObservablePoint CalculatePointByConfiguredStrategy(PartyMemberContext context, double plotSampleSize)
    {
        if (_config.IsPlotSlidingWindowEnabled)
        {
            int previousDamage = context.DamageHistory.GetFirst() ?? 0;
            double dps = (context.ViewModel.Damage - previousDamage) / plotSampleSize;
            return new ObservablePoint(_viewModel.TimeElapsed, dps);
        }

        double damage = _config.DamagePlotStrategy.Value switch
        {
            DamagePlotStrategy.TotalDamage => context.ViewModel.Damage,
            DamagePlotStrategy.DamagePerSecond => context.ViewModel.DPS,
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
            Fill = ColorFadeGradient.FromColor(
                color: AnalogousColor.NegativeFrom(
                    main: actualColor,
                    angle: 41.5
                )
            ),
            PointGeometry = null,
            Values = points,
            StrokeThickness = 1,
            LineSmoothness = 0
        };

        _viewModel.Series.Add(series);
        return series;
    }
}