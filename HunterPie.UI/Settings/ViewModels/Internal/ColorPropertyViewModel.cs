using HunterPie.Core.Settings.Types;

namespace HunterPie.UI.Settings.ViewModels.Internal;

internal class ColorPropertyViewModel(Color color) : ConfigurationPropertyViewModel
{
    public Color Color { get; } = color;
}