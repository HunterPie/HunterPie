using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Damage.ViewModels;

public class PetsViewModel : ViewModel
{
    public DamageMeterWidgetConfig Settings { get; }

    public PetsViewModel(DamageMeterWidgetConfig config)
    {
        Settings = config;
    }

    private int _totalDamage;
    public int TotalDamage { get => _totalDamage; set => SetValue(ref _totalDamage, value); }

    public ObservableCollection<PetViewModel> Members { get; } = new();

    public Observable<bool> ShouldHighlightMyself => Settings.ShouldHighlightMyself;
}