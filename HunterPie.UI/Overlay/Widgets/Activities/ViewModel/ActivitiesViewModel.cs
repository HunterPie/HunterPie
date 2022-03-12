using HunterPie.Core.Architecture;
using HunterPie.Core.Game.Client;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Activities.ViewModel
{
    public class ActivitiesViewModel : Bindable
    {
        public ObservableCollection<IActivity> Activities { get; } = new();
    }
}
