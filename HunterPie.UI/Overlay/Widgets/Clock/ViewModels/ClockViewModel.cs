using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Clock.ViewModels;

#nullable enable
public class ClockViewModel(ClockWidgetConfig config) : WidgetViewModel(config, "Clock Widget", WidgetType.ClickThrough)
{
    private readonly ClockWidgetConfig _config = config;

    public TimeOnly WorldTime { get; set => SetValue(ref field, value); }
    public TimeSpan? QuestTimeLeft { get; set => SetValue(ref field, value); }

    public ObservableCollection<MoonViewModel> Moons { get; } = new();
    public MoonViewModel? Moon { get; set => SetValue(ref field, value); }

    public Observable<bool> IsMoonPhaseEnabled => _config.IsMoonPhaseEnabled;
}