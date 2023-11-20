using HunterPie.Core.Settings.Types;

namespace HunterPie.UI.Settings.ViewModels.Internal;

public class PositionPropertyViewModel : ConfigurationPropertyViewModel
{
    public Position Position { get; }

    public PositionPropertyViewModel(Position position)
    {
        Position = position;
    }
}