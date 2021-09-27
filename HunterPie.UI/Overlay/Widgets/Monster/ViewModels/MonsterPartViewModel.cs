using HunterPie.Core.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Monster.ViewModels
{
    public class MonsterPartViewModel : Bindable
    {
        private string _name;
        private double _health;
        private double _maxHealth;
        private double _tenderize;
        private double _maxTenderize;

        public string Name { get => _name; set { SetValue(ref _name, value); } }
        public double Health { get => _health; set { SetValue(ref _health, value); } }
        public double MaxHealth { get => _maxHealth; set { SetValue(ref _maxHealth, value); } }
        public double Tenderize { get => _tenderize; set { SetValue(ref _tenderize, value); } }
        public double MaxTenderize { get => _maxTenderize; set { SetValue(ref _maxTenderize, value); } }
        public int Break { get; set; }
        public int MaxBreaks { get; set; }
    }
}
