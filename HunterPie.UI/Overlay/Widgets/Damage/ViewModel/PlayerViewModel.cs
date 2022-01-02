using HunterPie.Core.Architecture;
using HunterPie.Core.Game.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.UI.Overlay.Widgets.Damage.ViewModel
{
    public class PlayerViewModel : Bindable
    {

        public string Name { get; set; }
        public Weapon Weapon { get; set; }
        public int Damage { get; set; }
        public double DPS { get; set; }
        public double Percentage { get; set; }
        public bool IsIncreasing { get; set; }
        public bool IsUser { get; set; }

    }
}
