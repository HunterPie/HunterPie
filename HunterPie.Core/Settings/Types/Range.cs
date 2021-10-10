using HunterPie.Core.Architecture;

namespace HunterPie.Core.Settings.Types
{
    public class Range : Bindable
    {
        private double _current;
        public double Current { get => _current; set { SetValue(ref _current, value); } }

        private double _max;
        public double Max { get => _max; set { SetValue(ref _max, value); } }

        private double _min;
        public double Min { get => _min; set { SetValue(ref _min, value); } }

        private double _step;
        public double Step { get => _step; set { SetValue(ref _step, value); } }
    }
}
