using HunterPie.Core.Client.Configuration;
using HunterPie.Domain.Interfaces;
using HunterPie.Features.Extensions.Loader;
using HunterPie.UI.Main.Views;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class CustomThemeInitializer(
    ThemeLoaderService themeLoaderService,
    IConfiguration config,
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