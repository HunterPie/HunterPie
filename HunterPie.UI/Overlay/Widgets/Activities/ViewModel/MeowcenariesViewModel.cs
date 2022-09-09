using HunterPie.Core.Architecture;
using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Enums;

namespace HunterPie.UI.Overlay.Widgets.Activities.ViewModel
{
    public class MeowcenariesViewModel : Bindable, IActivity
    {
        private int _step;
        private int _maxSteps;
        private int _expectedOutcome;
        private int _maxOutcome;
        private int _buddyCount;
        private int _maxBuddyCount;
        private bool _isDeployed;
        private string _stepString;
        private string _outcomeString;
        private string _buddyString;
        
        public int Step { get => _step; set { SetValue(ref _step, value); } }
        public int MaxSteps { get => _maxSteps; set { SetValue(ref _maxSteps, value); } }
        public int ExpectedOutcome { get => _expectedOutcome; set { SetValue(ref _expectedOutcome, value); } }
        public int MaxOutcome { get => _maxOutcome; set { SetValue(ref _maxOutcome, value); } }
        public int BuddyCount { get => _buddyCount; set { SetValue(ref _buddyCount, value); } }
        public int MaxBuddyCount { get => _maxBuddyCount; set { SetValue(ref _maxBuddyCount, value); } }
        public bool IsDeployed { get => _isDeployed; set { SetValue(ref _isDeployed, value); } }
        public string StepString { get => _stepString; set => SetValue(ref _stepString, value + ":"); }
        public string OutcomeString { get => _outcomeString; set => SetValue(ref _outcomeString, value + ":"); }
        public string BuddyString { get => _buddyString; set => SetValue(ref _buddyString, value + ":"); }

        public ActivityType Type => ActivityType.Meowcenaries;
    }
}
