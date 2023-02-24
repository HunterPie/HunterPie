using HunterPie.UI.Architecture;
using LiveCharts;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HunterPie.GUI.Parts.Statistics.ViewModels;
public class HuntDetailsViewModel : ViewModel
{

    private PartyMemberDetailsViewModel _selectedPlayer = new();
    private int _monsterId;
    private DateTime _huntedAt;

    public ObservableCollection<PartyMemberDetailsViewModel> Players { get; } = new();
    public PartyMemberDetailsViewModel SelectedPlayer { get => _selectedPlayer; set => SetValue(ref _selectedPlayer, value); }
    public SeriesCollection DamageSeries { get; } = new();
    public SectionsCollection Sections { get; } = new();
    public int MonsterId { get => _monsterId; set => SetValue(ref _monsterId, value); }
    public DateTime HuntedAt { get => _huntedAt; set => SetValue(ref _huntedAt, value); }
    public float TimeElapsed { get; set; }

    public Func<double, string> TimeFormatter => new((value) => TimeSpan.FromSeconds(value).ToString("mm\\:ss"));

    public Func<double, string> DamageFormatter => new((damage) => $"{damage:0.00}/s");

    public void FilterOnly(PartyMemberDetailsViewModel member)
    {
        ISeriesView[] series = DamageSeries.ToArray();

        foreach (ISeriesView points in series)
            _ = DamageSeries.Remove(points);

        DamageSeries.Add(member.Series);

        SelectPlayer(member);
    }

    public void AddSections(List<AxisSection> sections) => Sections.AddRange(sections);

    public void RemoveSections(List<AxisSection> sections)
    {
        foreach (AxisSection section in sections)
            _ = Sections.Remove(section);
    }

    private void SelectPlayer(PartyMemberDetailsViewModel member) => SelectedPlayer = member;
}
