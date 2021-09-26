using HunterPie.Core.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.UI.Overlay.Widgets.Monster.ViewModels
{
    public class MonsterPartViewModel : Bindable
    {
        private string _name;
        private double _health;
        private double _maxHealth;


        public string Name { get => _name; set { SetValue(ref _name, value); } }
        public double Health { get => _health; set { SetValue(ref _health, value); } }
        public double MaxHealth { get => _maxHealth; set { SetValue(ref _maxHealth, value); } }
        public double Tenderize { get; set; }
        public double MaxTenderize { get; set; }
        public int Break { get; set; }
        public int MaxBreaks { get; set; }
    }
}
