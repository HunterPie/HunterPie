using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Activities;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Activities.Wilds.ViewModels;

public class MaterialRetrievalCollectorViewModel : ViewModel
{
    public MaterialRetrievalCollector Id { get; set => SetValue(ref field, value); }
    public int Count { get; set => SetValue(ref field, value); }
    public int MaxCount { get; set => SetValue(ref field, value); }
}