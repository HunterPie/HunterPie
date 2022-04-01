using HunterPie.Core.Architecture;
using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Enums;

namespace HunterPie.UI.Overlay.Widgets.Activities.ViewModel
{
    public class MeowcenariesViewModel : Bindable, IActivity
    {
        private int _daysLeft;
        private int _expectedOutcome;
        private int _buddyCount;
        private int _maxBuddyCount;
        
        public int DaysLeft { get => _daysLeft; set { SetValue(ref _daysLeft, value); } }
        public int MaxDays => 5;
        public int ExpectedOutcome { get => _expectedOutcome; set { SetValue(ref _expectedOutcome, value); } }
        public int BuddyCount { get => _buddyCount; set { SetValue(ref _buddyCount, value); } }
        public int MaxBuddyCount { get => _maxBuddyCount; set { SetValue(ref _maxBuddyCount, value); } }

        public ActivityType Type => ActivityType.Meowcenaries;
    }
}
