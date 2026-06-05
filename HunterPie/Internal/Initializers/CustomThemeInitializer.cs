using HunterPie.Core.Client.Configuration.Versions;
using HunterPie.Domain.Interfaces;
using HunterPie.Features.Theme.Loader;
using HunterPie.UI.Main.Views;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class CustomThemeInitializer(
    ThemeLoaderService themeLoaderService,
    V5Config config,
    MainView mainView
) : IInitializer
{

    public async Task Init()
    {
        await themeLoaderService.LoadAsync();

        config.Client.Themes.CollectionChanged += OnThemeCollectionChange;
    }

    private async void OnThemeCollectionChange(object? sender, NotifyCollectionChangedEventArgs e)
    {
        themeLoaderService.UnloadAllThemes();

        await themeLoaderService.LoadAllEnabledThemesAsync();

        mainView.Refresh();
    }
}