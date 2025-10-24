using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Activities;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Activities.Wilds.ViewModels;

public class MaterialRetrievalCollectorViewModel : ViewModel
{
    private MaterialRetrievalCollector _id;
    public MaterialRetrievalCollector Id { get => _id; set => SetValue(ref _id, value); }

    private int _count;
    public int Count { get => _count; set => SetValue(ref _count, value); }

    private int _maxCount;
    public int MaxCount { get => _maxCount; set => SetValue(ref _maxCount, value); }
}