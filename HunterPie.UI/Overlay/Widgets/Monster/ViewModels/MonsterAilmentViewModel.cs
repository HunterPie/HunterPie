using HunterPie.Core.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Monster.ViewModels
{
    public class MonsterAilmentViewModel : Bindable
    {
        public string _name;
        public double _timer;
        public double _maxTimer;
        public double _buildup;
        public double _maxBuildup;

        public string Name { get => _name; set { SetValue(ref _name, value); } }
        public double Timer { get => _timer; set { SetValue(ref _timer, value); }  }
        public double MaxTimer { get => _maxTimer; set { SetValue(ref _maxTimer, value); } }
        public double Buildup { get => _buildup; set { SetValue(ref _buildup, value); } }
        public double MaxBuildup { get => _maxBuildup; set { SetValue(ref _maxBuildup, value); } }

    }
}
