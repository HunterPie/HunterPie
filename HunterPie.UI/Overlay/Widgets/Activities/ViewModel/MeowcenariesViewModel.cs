using HunterPie.Core.Architecture;
using HunterPie.UI.Overlay.Widgets.Activities.Domain;

namespace HunterPie.UI.Overlay.Widgets.Activities.ViewModel
{
    public class MeowcenariesViewModel : Bindable, IActivity
    {
        private int _daysLeft;
        
        public int DaysLeft { get => _daysLeft; set { SetValue(ref _daysLeft, value); } }

        public ActivityType Type => ActivityType.Meowcenaries;
    }
}
