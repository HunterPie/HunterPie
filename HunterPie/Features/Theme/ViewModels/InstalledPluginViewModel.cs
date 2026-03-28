using HunterPie.Core.Architecture;
using HunterPie.Core.Plugins.Entity;
using HunterPie.UI.Architecture;

namespace HunterPie.Features.Theme.ViewModels;

internal class InstalledPluginViewModel(
    Plugin plugin
) : ViewModel
{

    public string Name => plugin.Manifest.Name;

    public string Description => plugin.Manifest.Description;

    public string Version => $"v{plugin.Manifest.Version}";

    public string Author => plugin.Manifest.Author;

    public Observable<bool> IsEnabled => plugin.Configuration.IsEnabled;
}
