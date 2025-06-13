using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Architecture;
using System;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Clock.ViewModels;

#nullable enable
public class ClockViewModel : ViewModel
{
    private readonly ClockWidgetConfig _config;

    private TimeOnly _worldTime;
    public TimeOnly WorldTime { get => _worldTime; set => SetValue(ref _worldTime, value); }

    private TimeSpan? _questTimeLeft;
    public TimeSpan? QuestTimeLeft { get => _questTimeLeft; set => SetValue(ref _questTimeLeft, value); }

    public ObservableCollection<MoonViewModel> Moons { get; } = new();

    private MoonViewModel? _moon;
    public MoonViewModel? Moon { get => _moon; set => SetValue(ref _moon, value); }

    public Observable<bool> IsMoonPhaseEnabled => _config.IsMoonPhaseEnabled;

    public ClockViewModel(ClockWidgetConfig config)
    {
        _config = config;
    }
}