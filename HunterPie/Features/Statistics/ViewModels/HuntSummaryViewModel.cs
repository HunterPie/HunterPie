using HunterPie.UI.Architecture;
using LiveCharts;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace HunterPie.Features.Statistics.ViewModels;
public class HuntSummaryViewModel : ViewModel
{

    private int _monsterId;
    private DateTime _huntedAt;

    public ObservableCollection<PartyMemberSummaryViewModel> Players { get; } = new();
    public SeriesCollection DamageSeries { get; } = new();
    public SectionsCollection EnrageSections { get; } = new();
    public int MonsterId { get => _monsterId; set => SetValue(ref _monsterId, value); }
    public DateTime HuntedAt { get => _huntedAt; set => SetValue(ref _huntedAt, value); }
    public float TimeElapsed { get; set; }

    public Func<double, string> TimeFormatter { get; set; } =
        new Func<double, string>((value) => TimeSpan.FromSeconds(value).ToString("mm\\:ss"));

    public Func<double, string> DamageFormatter { get; set; } = new Func<double, string>((damage) => $"{damage:0.00}/s");

    public void FilterOnly(PartyMemberSummaryViewModel member)
    {
        ISeriesView[] series = DamageSeries.ToArray();

        foreach (ISeriesView points in series)
            _ = DamageSeries.Remove(points);

        DamageSeries.Add(member.Series);
    }
}
