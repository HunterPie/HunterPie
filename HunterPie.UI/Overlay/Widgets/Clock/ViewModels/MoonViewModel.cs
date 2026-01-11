using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Game.Moon;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Clock.ViewModels;

public class MoonViewModel : ViewModel
{
    public MoonPhase Phase { get; set => SetValue(ref field, value); }
    public bool IsActive { get; set => SetValue(ref field, value); }
}