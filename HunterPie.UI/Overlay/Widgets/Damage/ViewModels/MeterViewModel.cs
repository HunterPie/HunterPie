using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Extensions;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.ViewModels;
using LiveCharts;
using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace HunterPie.UI.Overlay.Widgets.Damage.ViewModels;

public class MeterViewModel : WidgetViewModel
{
    public DamageMeterWidgetConfig Config { get; }

    public Func<double, string> TimeFormatter { get; set; } =
        new Func<double, string>((value) => TimeSpan.FromSeconds(value).ToString("mm\\:ss"));

    public Func<double, string> DamageFormatter { get; set => SetValue(ref field, value); }

    public SeriesCollection Series { get; protected set; } = new();

    public ObservableCollection<PlayerViewModel> Players { get; } = new();

    public PetsViewModel Pets { get; }

    public double TimeElapsed
    {
        get;
        set => SetValue(ref field, value);
    } = 1;

    public int MaxDeaths
    {
        get;
        set => SetValue(ref field, value);
    }

    public int Deaths
    {
        get;
        set => SetValue(ref field, value);
    }

    public bool InHuntingZone { get; set => SetValue(ref field, value); }

    public bool HasPetsToBeDisplayed { get; set => SetValue(ref field, value); }

    public MeterViewModel(DamageMeterWidgetConfig config) : base(config, "Damage Meter Widget", WidgetType.ClickThrough)
    {
        Config = config;
        Pets = new(config);
        SetupFormatters();
    }

    private void SetupFormatters() => DamageFormatter = FormatDamageByStrategy;

    private string FormatDamageByStrategy(double damage)
    {
        return Config.DamagePlotStrategy.Value switch
        {
            DamagePlotStrategy.TotalDamage => damage.ToString(CultureInfo.InvariantCulture),
            DamagePlotStrategy.DamagePerSecond
                or DamagePlotStrategy.MovingAverageDamagePerSecond => $"{damage:0.00}/s",
            _ => throw new NotImplementedException()
        };
    }

    public void ToggleHighlight() => Config.ShouldHighlightMyself.Value = !Config.ShouldHighlightMyself;

    public void ToggleBlur() => Config.ShouldBlurNames.Value = !Config.ShouldBlurNames;

    public void SortMembers()
    {
        Players.SortInPlace(player => player.Damage);
        Pets.Members.SortInPlace(pet => pet.DamageBar.Percentage);
    }
}