using HunterPie.Core.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Abnormality.ViewModel
{
    public class AbnormalityViewModel : Bindable
    {
        private string _icon;
        public string Icon
        {
            get { return _icon; }
            set { SetValue(ref _icon, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetValue(ref _name, value); }
        }

        private double _timer;
        public double Timer
        {
            get { return _timer; }
            set { SetValue(ref _timer, value); }
        }

        private double _maxTimer;
        public double MaxTimer
        {
            get { return _maxTimer; }
            set { SetValue(ref  _maxTimer, value); }
        }

        private bool _isBuff;
        public bool IsBuff
        {
            get { return _isBuff; }
            set { SetValue(ref _isBuff, value); }
        }

        public bool _isBuildup;
        public bool IsBuildup
        {
            get { return _isBuildup; }
            set { SetValue(ref _isBuildup, value); }
        }
    }
}
