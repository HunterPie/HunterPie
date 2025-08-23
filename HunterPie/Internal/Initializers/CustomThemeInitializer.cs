using HunterPie.Core.Client.Configuration.Versions;
using HunterPie.Domain.Interfaces;
using HunterPie.Features.Theme.Loader;
using HunterPie.Features.Theme.Repository;
using HunterPie.UI.Main.Views;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class CustomThemeInitializer : IInitializer
{
    private readonly LocalThemeRepository _localThemeRepository;
    private readonly ThemeLoaderService _themeLoaderService;
    private readonly V5Config _config;
    private readonly MainView _mainView;

    public CustomThemeInitializer(
        ThemeLoaderService themeLoaderService,
        V5Config config,
        LocalThemeRepository localThemeRepository,
        MainView mainView)
    {
        _themeLoaderService = themeLoaderService;
        _config = config;
        _localThemeRepository = localThemeRepository;
        _mainView = mainView;
    }

    public async Task Init()
    {
        await _themeLoaderService.LoadAsync();

        _config.Client.Themes.CollectionChanged += OnThemeCollectionChange;
    }

    private async void OnThemeCollectionChange(object? sender, NotifyCollectionChangedEventArgs e)
    {
        _themeLoaderService.UnloadAllThemes();

        await _themeLoaderService.LoadAllEnabledThemesAsync();

        /*switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems is not { } added)
                    return;

                foreach (object? element in added)
                {
                    if (element is not string themeId)
                        continue;

                    LocalThemeManifest? manifest = _localThemeRepository.FindBy(themeId);

                    if (manifest is not { })
                        continue;

                    _themeLoaderService.LoadTheme(manifest);
                }

                break;
            case NotifyCollectionChangedAction.Remove:
                if (e.OldItems is not { } removed)
                    return;

                foreach (object? element in removed)
                {
                    if (element is not string themeId)
                        continue;

                    LocalThemeManifest? manifest = _localThemeRepository.FindBy(themeId);

                    if (manifest is not { })
                        continue;

                    _themeLoaderService.UnloadTheme(manifest);
                }
                break;
        }*/

        _mainView.Reload();
    }
}