using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Game.Moon;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Clock.ViewModels;

public class MoonViewModel : ViewModel
{
    private MoonPhase _phase;
    public MoonPhase Phase { get => _phase; set => SetValue(ref _phase, value); }

    private bool _isActive;
    public bool IsActive { get => _isActive; set => SetValue(ref _isActive, value); }
}