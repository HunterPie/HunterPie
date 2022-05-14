using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.SpecializedTools.ViewModels
{
    public class SpecializedToolViewModel : ViewModel
    {
        private double _percentage;
        public double Percentage { get => _percentage; set { SetValue(ref _percentage, value); } }
    }
}
