using HunterPie.Core.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.UI.Overlay.Widgets.Monster.ViewModels
{
    public class MonsterAilmentViewModel : Notifiable
    {
        public string Name { get; set; }
        public float Timer { get; set; }
        public float MaxTimer { get; set; }
        public float Buildup { get; set; }
        public float MaxBuildup { get; set; }

    }
}
