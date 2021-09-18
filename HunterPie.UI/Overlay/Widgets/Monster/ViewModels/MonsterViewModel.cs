using HunterPie.Core.Architecture;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.UI.Overlay.Widgets.Monster.ViewModels
{
    public class MonsterViewModel : Notifiable
    {
        // Monster data
        public string Name { get; set; }
        public string Icon { get; set; }
        public float Health { get; set; }
        public float MaxHealth { get; set; }
        public float Stamina { get; set; }
        public float MaxStamina { get; set; } 
        public readonly ObservableCollection<MonsterPartViewModel> Parts = new();
        public readonly ObservableCollection<MonsterAilmentViewModel> Ailments = new();

        // Monster states
        public bool IsEnraged { get; set; }
        public bool IsTarget { get; set; }
    }
}
