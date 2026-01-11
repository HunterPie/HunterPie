using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Damage.ViewModels;

public class PetsViewModel(DamageMeterWidgetConfig config) : ViewModel
{
    public DamageMeterWidgetConfig Settings { get; } = config;
    public int TotalDamage { get; set => SetValue(ref field, value); }

    public ObservableCollection<PetViewModel> Members { get; } = new();

    public Observable<bool> ShouldHighlightMyself => Settings.ShouldHighlightMyself;
}