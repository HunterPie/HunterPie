using HunterPie.Core.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Wirebug.ViewModel
{
    public class WirebugViewModel : Bindable
    {

        private double _cooldown;
        private double _maxCooldown;
        private double _timer;
        private double _maxTimer;
        private bool _onCooldown;

        public double Cooldown { get => _cooldown; set { SetValue(ref _cooldown, value); } }
        public double MaxCooldown { get => _maxCooldown; set { SetValue(ref _maxCooldown, value); } }
        public double Timer { get => _timer; set { SetValue(ref _timer, value); } }
        public double MaxTimer { get => _maxTimer; set { SetValue(ref _maxTimer, value); } }
        public bool OnCooldown { get => _onCooldown; set { SetValue(ref _onCooldown, value); } }
        
    }
}
