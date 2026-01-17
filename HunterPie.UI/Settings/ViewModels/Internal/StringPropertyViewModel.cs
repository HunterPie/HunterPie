using HunterPie.Core.Architecture;

namespace HunterPie.UI.Settings.ViewModels.Internal;

#nullable enable
internal class StringPropertyViewModel(Observable<string> @string) : ConfigurationPropertyViewModel
{
    public Observable<string> String { get; } = @string;
}