using HunterPie.Core.Extensions;
using HunterPie.Core.Settings.Types;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Settings.ViewModels.Internal;

internal class FileSelectorPropertyViewModel(IFileSelector fileSelector) : ConfigurationPropertyViewModel
{
    public IFileSelector FileSelector { get; } = fileSelector;

    public ObservableCollection<string> Elements { get; } = fileSelector.GetElements().ToObservableCollection();
}