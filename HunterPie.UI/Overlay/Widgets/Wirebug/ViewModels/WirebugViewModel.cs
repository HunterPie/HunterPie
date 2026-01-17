using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enums;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Wirebug.ViewModels;

public class WirebugViewModel : ViewModel
{
    public double Cooldown { get; set => SetValue(ref field, value); }
    public double MaxCooldown { get; set => SetValue(ref field, value); }
    public double Timer { get; set => SetValue(ref field, value); }
    public double MaxTimer { get; set => SetValue(ref field, value); }
    public bool OnCooldown { get; set => SetValue(ref field, value); }
    public bool IsAvailable { get; set => SetValue(ref field, value); }
    public bool IsTemporary { get; set => SetValue(ref field, value); }
    public WirebugState WirebugState { get; set => SetValue(ref field, value); }
}