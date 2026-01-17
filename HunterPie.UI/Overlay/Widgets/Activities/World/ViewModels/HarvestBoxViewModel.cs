using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Activities.World.ViewModels;

public class HarvestBoxViewModel : ViewModel
{
    public ObservableCollection<HarvestFertilizerViewModel> Fertilizers { get; } = new();
    public int Count { get; set => SetValue(ref field, value); }
    public int MaxCount { get; set => SetValue(ref field, value); }
}