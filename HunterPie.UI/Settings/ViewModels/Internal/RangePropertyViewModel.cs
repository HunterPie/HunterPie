using HunterPie.Core.Settings.Types;

namespace HunterPie.UI.Settings.ViewModels.Internal;

internal class RangePropertyViewModel(Range range) : ConfigurationPropertyViewModel
{
    public Range Range { get; } = range;
}