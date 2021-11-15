using HunterPie.Core.Architecture;

namespace HunterPie.Core.Settings.Types
{
    public class Secret : Bindable
    {
        private string _value;

        public string Value
        {
            get => _value;
            set { SetValue(ref _value, value); }
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
