using HunterPie.Core.Game.Events;
using HunterPie.Core.List;
using HunterPie.UI.Overlay.Widgets.Damage.ViewModels;
using LiveCharts.Wpf;

namespace HunterPie.UI.Overlay.Widgets.Damage.Controllers;

#nullable enable
internal class PartyMemberContext
{
    public required PlayerViewModel ViewModel { get; init; }
    public required Series Plots { get; init; }
    public double JoinedAt { get; set; }
    public double FirstHitAt { get; set; }
    public required SlidingWindow<int> DamageHistory { get; set; }

    public void OnAffinityChanged(object? sender, SimpleValueChangeEventArgs<double> e)
    {
        ViewModel.Affinity = e.NewValue;
    }

    public void OnElementalDamageChanged(object? sender, SimpleValueChangeEventArgs<double> e)
    {
        ViewModel.ElementalDamage = e.NewValue;
    }

    public void OnRawDamageChanged(object? sender, SimpleValueChangeEventArgs<double> e)
    {
        ViewModel.RawDamage = e.NewValue;
    }
}