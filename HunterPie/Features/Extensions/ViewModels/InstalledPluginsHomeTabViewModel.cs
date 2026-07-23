using HunterPie.Core.Client;
using HunterPie.Core.Plugins.Entity;
using HunterPie.Core.System;
using HunterPie.DI;
using HunterPie.Features.Plugins.Repository;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Features.Extensions.ViewModels;

internal class InstalledPluginsHomeTabViewModel(
    IPluginRepository pluginRepository
) : ThemeHomeTabViewModel
{

    public ObservableCollection<InstalledPluginViewModel> Plugins { get; } = new();

    public bool IsRefreshing { get; set => SetValue(ref field, value); }

    public async Task RefreshAsync()
    {
        IsRefreshing = true;

        IEnumerable<Plugin> plugins = pluginRepository.FindAll().Select(it => it.Plugin);

        Plugins.Clear();

        foreach (Plugin plugin in plugins)
        {
            InstalledPluginViewModel pluginViewModel = DependencyContainer.Get<InstalledPluginViewModel>(@override:
                local => local.WithSingle(_ => plugin)
            );
            Plugins.Add(pluginViewModel);
        }

        IsRefreshing = false;
    }

    public void OpenFolder()
    {
        BrowserService.OpenFolder(
            path: ClientInfo.PluginsPath
        );
    }
}