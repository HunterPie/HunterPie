using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Damage.ViewModels;

public class PetViewModel : ViewModel
{
    private string _name = string.Empty;
    public string Name { get => _name; set => SetValue(ref _name, value); }

    public DamageBarViewModel DamageBar { get; set; }

    public PetViewModel(DamageBarViewModel damageBar)
    {
        DamageBar = damageBar;
    }
}