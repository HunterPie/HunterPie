using HunterPie.Core.Architecture;

namespace HunterPie.UI.Settings.ViewModels.Internal;

internal class BooleanPropertyViewModel(Observable<bool> value) : ConfigurationPropertyViewModel
{
    public Observable<bool> Boolean { get; } = value;
}