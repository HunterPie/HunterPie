using HunterPie.Core.Architecture;
using System.Threading.Tasks;
using System.Timers;

namespace HunterPie.UI.Overlay.Widgets.Monster.ViewModels
{
    public class MonsterAilmentViewModel : Bindable
    {
        // Internal logic
        private readonly Timer timeout = new(15000);

        private string _name;
        private double _timer;
        private double _maxTimer;
        private double _buildup;
        private double _maxBuildup;
        private int _count;
        private bool _isActive;

        public string Name { get => _name; set { SetValue(ref _name, value); } }
        public double Timer
        {
            get => _timer;
            set
            {
                if (value != _timer)
                    RefreshTimeout();

                SetValue(ref _timer, value);
            }
        }
        public double MaxTimer { get => _maxTimer; set { SetValue(ref _maxTimer, value); } }
        public double Buildup
        {
            get => _buildup;
            set
            {
                if (value != _buildup)
                    RefreshTimeout();

                SetValue(ref _buildup, value);
            }
        }
        public double MaxBuildup { get => _maxBuildup; set { SetValue(ref _maxBuildup, value); } }
        public int Count { get => _count; set { SetValue(ref _count, value); } }
        public bool IsActive { get => _isActive; set { SetValue(ref _isActive, value); } }

        public MonsterAilmentViewModel()
        {
            timeout.Elapsed += UpdateActiveState;
            timeout.Start();
        }

        private void UpdateActiveState(object sender, ElapsedEventArgs e) => IsActive = false;

        private void RefreshTimeout()
        {
            IsActive = true;
            timeout.Start();
        }
    }
}
