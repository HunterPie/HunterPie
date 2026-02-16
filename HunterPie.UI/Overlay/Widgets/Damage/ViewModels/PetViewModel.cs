using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Damage.ViewModels;

public class PetViewModel(DamageBarViewModel damageBar) : ViewModel
{
    public string Name { get; set => SetValue(ref field, value); } = string.Empty;

    public DamageBarViewModel DamageBar { get; set; } = damageBar;
}