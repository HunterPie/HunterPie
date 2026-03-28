using HunterPie.Core.Plugins.Entity;
using HunterPie.Features.Plugins.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HunterPie.Features.Theme.ViewModels;

internal class InstalledPluginsHomeTabViewModel(
    PluginProvider pluginProvider
) : ThemeHomeTabViewModel
{

    private ObservableCollection<InstalledPluginViewModel> Plugins { get; } = new();

    public bool IsRefreshing { get; set => SetValue(ref field, value); }

    public async Task RefreshAsync()
    {
        IsRefreshing = true;

        IReadOnlyList<Plugin> plugins = pluginProvider.Get();

        Plugins.Clear();

        foreach (Plugin plugin in plugins)
        {
            Plugins.Add(new InstalledPluginViewModel(plugin));
        }

        IsRefreshing = false;
    }
}
