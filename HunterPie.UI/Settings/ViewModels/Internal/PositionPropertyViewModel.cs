using HunterPie.Core.Settings.Types;

namespace HunterPie.UI.Settings.ViewModels.Internal;

internal class PositionPropertyViewModel(Position position) : ConfigurationPropertyViewModel
{
    public Position Position { get; } = position;
}