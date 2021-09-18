using HunterPie.Core.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.UI.Overlay.Widgets.Monster.ViewModels
{
    public class MonsterPartViewModel : Notifiable
    {
        public string Name { get; set; }
        public float Health { get; set; }
        public float MaxHealth { get; set; }
        public float Tenderize { get; set; }
        public float MaxTenderize { get; set; }
        public int Break { get; set; }
        public int MaxBreaks { get; set; }
    }
}
