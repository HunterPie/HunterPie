using HunterPie.Core.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Abnormality.ViewModel
{
    public class AbnormalityBarViewModel : Bindable
    {
        
        private readonly ObservableCollection<AbnormalityViewModel> _abnormalities = new ObservableCollection<AbnormalityViewModel>();

        public ObservableCollection<AbnormalityViewModel> Abnormalities => _abnormalities;

    }
}
