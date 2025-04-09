using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Enums;
using HunterPie.Features.Statistics.Details.Enums;
using HunterPie.UI.Architecture;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HunterPie.Features.Statistics.Details.ViewModels;

internal class MonsterDetailsViewModel : ViewModel
{
    private bool _isInitialized;

    private string _name = string.Empty;
    public string Name { get => _name; set => SetValue(ref _name, value); }

    private string _icon = string.Empty;
    public string Icon { get => _icon; set => SetValue(ref _icon, value); }

    private TimeSpan? _huntElapsed;
    public TimeSpan? HuntElapsed { get => _huntElapsed; set => SetValue(ref _huntElapsed, value); }

    private TimeSpan _timeElapsed;
    public TimeSpan TimeElapsed { get => _timeElapsed; set => SetValue(ref _timeElapsed, value); }

    private Crown _crown;
    public Crown Crown { get => _crown; set => SetValue(ref _crown, value); }

    private double _maxHealth;
    public double MaxHealth { get => _maxHealth; set => SetValue(ref _maxHealth, value); }

    private PlotStrategy _plotStrategy;
    public PlotStrategy PlotStrategy { get => _plotStrategy; set => SetValue(ref _plotStrategy, value); }

    public ObservableCollection<PartyMemberDetailsViewModel> Players { get; init; } = new();

    public ObservableCollection<AbnormalityDetailsViewModel> SelectedAbnormalities { get; init; } = new();

    public ObservableCollection<StatusDetailsViewModel> Statuses { get; init; } = new();

    public SeriesCollection DamageSeries { get; } = new();

    public SectionsCollection Sections { get; } = new();

    public Func<double, string> TimeFormatter => new((value) => TimeSpan.FromSeconds(value).ToString("mm\\:ss"));

    public Func<double, string> DamageFormatter => new((damage) => $"{damage:0.00}/s");

    public void SetupView()
    {
        EnableEnrageSections();

        Players.ForEach(it =>
        {
            if (!_isInitialized)
                it.IsToggled = true;
        });

        PopulateSeries();

        SelectedAbnormalities.Clear();

        Players.Where(it => it.Abnormalities.Any())
               .SelectMany(it => it.Abnormalities)
               .ForEach(it => SelectedAbnormalities.Add(it));

        SelectedAbnormalities.Where(it => it.IsToggled)
                             .ForEach(it => ToggleSections(it, true));

        _isInitialized = true;
    }

    public void PopulateSeries()
    {
        DamageSeries.Clear();

        IEnumerable<Series> series = Players.Where(it => it.IsToggled)
            .Select(it => it.CalculateSeries(PlotStrategy));

        DamageSeries.AddRange(series);
    }

    public void ToggleMember(PartyMemberDetailsViewModel player)
    {
        player.IsToggled = !player.IsToggled;

        PopulateSeries();
    }

    public void ToggleSections(ISectionControllable controllable, bool? state = null)
    {
        controllable.IsToggled = state ?? !controllable.IsToggled;

        if (controllable.IsToggled)
            Sections.AddRange(controllable.Activations);
        else
            controllable.Activations.ForEach(it => Sections.Remove(it));
    }

    private void EnableEnrageSections()
    {
        StatusDetailsViewModel? enrage = Statuses.FirstOrDefault();

        if (enrage is not { })
            return;

        ToggleSections(enrage, !_isInitialized || enrage.IsToggled);
    }
}