using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Damage.ViewModels
{
    public class PetsViewModel : ViewModel
    {
        private string _name;
        private int _totalDamage;

        public string Name { get => _name; set => SetValue(ref _name, value); }
        public int TotalDamage { get => _totalDamage; set => SetValue(ref _totalDamage, value); }
        public ObservableCollection<DamageBarViewModel> Damages { get; } = new();
    }
}
