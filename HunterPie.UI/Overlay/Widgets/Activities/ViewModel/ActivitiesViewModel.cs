using HunterPie.Core.Architecture;
using HunterPie.UI.Overlay.Widgets.Activities.Domain;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Activities.ViewModel
{
    public class ActivitiesViewModel : Bindable
    {
        public ObservableCollection<IActivity> Activities { get; } = new();
    }
}
