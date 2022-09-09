using HunterPie.Core.Architecture;
using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Enums;

namespace HunterPie.UI.Overlay.Widgets.Activities.ViewModel
{
    public class CohootNestViewModel : Bindable, IActivity
    {
        private int _count;
        private int _maxCount;

        public int Count { get => _count; set { SetValue(ref _count, value); } }
        public int MaxCount { get => _maxCount; set { SetValue(ref _maxCount, value); } }

        public ActivityType Type => ActivityType.Cohoot;
    }
}
