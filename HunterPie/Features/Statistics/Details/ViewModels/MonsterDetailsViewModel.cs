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

    public string Name { get; set => SetValue(ref field, value); } = string.Empty;
    public string Icon { get; set => SetValue(ref field, value); } = string.Empty;
    public TimeSpan? HuntElapsed { get; set => SetValue(ref field, value); }
    public TimeSpan TimeElapsed { get; set => SetValue(ref field, value); }
    public Crown Crown { get; set => SetValue(ref field, value); }
    public double MaxHealth { get; set => SetValue(ref field, value); }
    public PlotStrategy PlotStrategy { get; set => SetValue(ref field, value); }

    public ObservableCollection<PartyMemberDetailsViewModel> Players { get; init; } = new();

    public ObservableCollection<AbnormalityDetailsViewModel> SelectedAbnormalities { get; init; } = new();

    public ObservableCollection<StatusDetailsViewModel> Statuses { get; init; } = new();

    public SeriesCollection DamageSeries { get; } = new();

    public SectionsCollection Sections { get; } = new();

    public required Series? HealthSteps { get; init; }

    public Func<double, string> TimeFormatter { get; } = (value) => TimeSpan.FromSeconds(value).ToString("mm\\:ss");

    public Func<double, string> DamageFormatter { get; } = (damage) => $"{damage:0.00}/s";

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

        if (HealthSteps is not null)
            DamageSeries.Add(HealthSteps);
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

    public void ToggleHealthSteps()
    {
        if (DamageSeries.Contains(HealthSteps))
            DamageSeries.Remove(HealthSteps);
        else
            DamageSeries.Add(HealthSteps);
    }
}