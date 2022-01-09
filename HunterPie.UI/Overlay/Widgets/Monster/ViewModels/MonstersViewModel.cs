using HunterPie.Core.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Monster.ViewModels
{
    public class MonstersViewModel : Bindable
    {
        private readonly ObservableCollection<BossMonsterViewModel> _monsters = new();

        public ObservableCollection<BossMonsterViewModel> Monsters => _monsters;

    }
}
