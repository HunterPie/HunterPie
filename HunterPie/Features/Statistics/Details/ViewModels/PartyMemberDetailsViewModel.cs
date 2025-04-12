using HunterPie.Core.Game.Enums;
using HunterPie.Core.List;
using HunterPie.Features.Statistics.Details.Enums;
using HunterPie.Features.Statistics.Models;
using HunterPie.UI.Architecture;
using HunterPie.UI.Architecture.Brushes;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace HunterPie.Features.Statistics.Details.ViewModels;

internal class PartyMemberDetailsViewModel : ViewModel
{
    private const int SLIDING_WINDOW_SIZE = 10;

    private readonly HuntStatisticsModel _hunt;
    private readonly MonsterModel _monster;
    private readonly IReadOnlyCollection<PlayerDamageFrameModel> _damageList;
    private readonly Color _primitiveColor;

    private string _name = string.Empty;
    public string Name { get => _name; set => SetValue(ref _name, value); }

    private Weapon _weapon;
    public Weapon Weapon { get => _weapon; set => SetValue(ref _weapon, value); }

    private Brush _color = Brushes.Transparent;
    public Brush Color { get => _color; set => SetValue(ref _color, value); }

    private float _damage;
    public float Damage { get => _damage; set => SetValue(ref _damage, value); }

    private double _contribution;
    public double Contribution { get => _contribution; set => SetValue(ref _contribution, value); }

    private bool _isToggled;
    public bool IsToggled { get => _isToggled; set => SetValue(ref _isToggled, value); }

    public required ObservableCollection<AbnormalityDetailsViewModel> Abnormalities { get; init; }

    public PartyMemberDetailsViewModel(
        HuntStatisticsModel hunt,
        MonsterModel monster,
        IReadOnlyCollection<PlayerDamageFrameModel> damageList,
        Color primitiveColor)
    {
        _damageList = damageList;
        _primitiveColor = primitiveColor;
        _monster = monster;
        _hunt = hunt;
    }

    public Series CalculateSeries(PlotStrategy strategy)
    {
        IEnumerable<ObservablePoint> points = strategy switch
        {
            PlotStrategy.AverageMoving => CalculateMovingAverage(),
            PlotStrategy.AverageOverall => CalculateAverage(),
            _ => throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null)
        };

        return new LineSeries
        {
            Title = Name,
            Stroke = Color,
            Fill = ColorFadeGradient.FromColor(_primitiveColor),
            PointGeometry = null,
            StrokeThickness = 1,
            LineSmoothness = 0,
            Values = new ChartValues<ObservablePoint>(points)
        };
    }

    private IEnumerable<ObservablePoint> CalculateAverage()
    {
        float accumulatedDamage = 0;
        return _damageList.Select(it => (it.DealtAt, Damage: accumulatedDamage = it.Damage + accumulatedDamage))
            .Select(it =>
            {
                double time = Math.Max(1.0, (it.DealtAt - _hunt.StartedAt).TotalSeconds);

                return new ObservablePoint
                {
                    X = time,
                    Y = it.Damage / time
                };
            });
    }

    private IEnumerable<ObservablePoint> CalculateMovingAverage()
    {
        DateTime huntStartedAt = _monster.HuntStartedAt!.Value;
        DateTime huntFinishedAt = _monster.HuntFinishedAt!.Value;

        var points = _damageList.GroupBy(it => (int)Math.Floor((it.DealtAt - _hunt.StartedAt).TotalSeconds))
            .Select(it => (it.Key, it.Sum(model => model.Damage)))
            .OrderBy(it => it.Key)
            .ToList();
        int endSecond = (int)(huntFinishedAt - huntStartedAt).TotalSeconds;

        List<float> normalizedPoints = new(endSecond);

        float lastDamage = 0.0f;
        int index = 0;
        for (int secondElapsed = 0; secondElapsed < endSecond; secondElapsed++)
        {
            if (index >= points.Count)
                break;

            (int elapsed, float damage) = points[index];

            while (secondElapsed < elapsed)
            {
                normalizedPoints.Add(lastDamage);
                secondElapsed++;
            }

            lastDamage = elapsed != secondElapsed
                ? lastDamage
                : lastDamage + damage;

            normalizedPoints.Add(lastDamage);
            index++;
        }

        var window = new SlidingWindow<float>(SLIDING_WINDOW_SIZE);

        var observablePoints = new List<ObservablePoint>(normalizedPoints.Count / SLIDING_WINDOW_SIZE);

        double timeElapsed = (huntStartedAt - _hunt.StartedAt).TotalSeconds;
        foreach (float damage in normalizedPoints)
        {
            window.Add(damage);

            float first = window.GetFirst() ?? 0.0f;
            float last = window.GetLast() ?? 0.0f;

            observablePoints.Add(new ObservablePoint(timeElapsed, (last - first) / SLIDING_WINDOW_SIZE));
            timeElapsed++;
        }

        return observablePoints;
    }
}