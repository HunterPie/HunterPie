using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Activities.Wilds.ViewModels;

public class MaterialRetrievalViewModel : ViewModel
{
    public ObservableCollection<MaterialRetrievalCollectorViewModel> Collectors { get; } = new();
}