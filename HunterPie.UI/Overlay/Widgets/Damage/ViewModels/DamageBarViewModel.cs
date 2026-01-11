using HunterPie.Core.Settings.Types;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Damage.ViewModels;

public class DamageBarViewModel(Color color) : ViewModel
{
    public Color Color { get; set => SetValue(ref field, value); } = color;
    public double Percentage { get; set => SetValue(ref field, value); }
}