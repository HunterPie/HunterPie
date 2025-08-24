﻿using HunterPie.Core.Client.Configuration.Versions;
using HunterPie.Domain.Interfaces;
using HunterPie.Features.Theme.Loader;
using HunterPie.UI.Main.Views;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class CustomThemeInitializer : IInitializer
{
    private readonly ThemeLoaderService _themeLoaderService;
    private readonly V5Config _config;
    private readonly MainView _mainView;

    public CustomThemeInitializer(
        ThemeLoaderService themeLoaderService,
        V5Config config,
        MainView mainView)
    {
        _themeLoaderService = themeLoaderService;
        _config = config;
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

        _mainView.Refresh();
    }
}