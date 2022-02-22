using HunterPie.Core.Architecture;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.UI.Overlay.Widgets.Wirebug.ViewModel
{
    public class WirebugsViewModel : Bindable
    {
        private ObservableCollection<WirebugViewModel> _elements = new();
        
        public ObservableCollection<WirebugViewModel> Elements => _elements;

    }
}
