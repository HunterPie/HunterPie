using HunterPie.Core.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Activities.ViewModel;

public class HarvestBoxViewModel : Bindable
{
    public ObservableCollection<HarvestFertilizerViewModel> Fertilizers { get; } = new();

    private int _count;
    public int Count { get => _count; set => SetValue(ref _count, value); }

    private int _maxCount;
    private int MaxCount { get => _maxCount; set => SetValue(ref _maxCount, value); }
}