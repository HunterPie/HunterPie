using HunterPie.Core.Settings.Types;

namespace HunterPie.UI.Settings.ViewModels.Internal;

internal class FileSelectorPropertyViewModel : ConfigurationPropertyViewModel
{
    public IFileSelector FileSelector { get; }

    public FileSelectorPropertyViewModel(IFileSelector fileSelector)
    {
        FileSelector = fileSelector;
    }
}