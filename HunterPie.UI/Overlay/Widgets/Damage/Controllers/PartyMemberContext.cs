using HunterPie.Core.List;
using HunterPie.UI.Overlay.Widgets.Damage.ViewModels;
using LiveCharts.Wpf;

namespace HunterPie.UI.Overlay.Widgets.Damage.Controllers;

internal class PartyMemberContext
{
    public required PlayerViewModel ViewModel { get; init; }
    public required Series Plots { get; init; }
    public double JoinedAt { get; set; }
    public double FirstHitAt { get; set; }
    public required SlidingWindow<int> DamageHistory { get; set; }
}