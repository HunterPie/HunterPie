using HunterPie.Core.Architecture;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.UI.Overlay.Widgets.Damage.ViewModel
{
    public class MeterViewModel : Bindable
    {
        private readonly ObservableCollection<PlayerViewModel> _players = new();

        public ObservableCollection<PlayerViewModel> Players => _players;
    }
}
