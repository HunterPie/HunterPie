﻿using HunterPie.Core.Architecture;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Extensions;
using HunterPie.Core.Settings;
using HunterPie.Features.Overlay.Services;
using HunterPie.Features.Settings.ViewModels;
using HunterPie.Integrations.Poogie.Version;
using HunterPie.UI.Architecture;
using HunterPie.UI.Navigation;
using HunterPie.UI.Settings;
using HunterPie.UI.Settings.Converter.Model;
using HunterPie.UI.Settings.ViewModels.Internal;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace HunterPie.Features.Debug.ViewModels;

internal class DebugOverlayManagerViewModel : ViewModel
{
    private readonly ConfigurationAdapter _configurationAdapter;
    private readonly PoogieVersionConnector _poogieVersionConnector;
    private readonly IBodyNavigator _bodyNavigator;

    public BooleanPropertyViewModel IsDesignModeEnabled
    { get; }
    public BooleanPropertyViewModel IsGameFocused { get; }
    public BooleanPropertyViewModel IsGameHudOpen { get; }

    public ObservableCollection<IWidgetSettings> Settings { get; }

    public DebugOverlayManagerViewModel(
        OverlayManager manager,
        ConfigurationAdapter configurationAdapter,
        PoogieVersionConnector poogieVersionConnector,
        IBodyNavigator bodyNavigator,
        ObservableCollection<IWidgetSettings> settings)
    {
        _configurationAdapter = configurationAdapter;
        _poogieVersionConnector = poogieVersionConnector;
        _bodyNavigator = bodyNavigator;
        IsDesignModeEnabled = CreateBooleanObservable(
            name: "Is design mode enabled",
            description: "Simulates design mode",
            callback: state => manager.IsDesignModeEnabled = state
        );
        IsGameFocused = CreateBooleanObservable(
            name: "Is game focused",
            description: "Simulates game window focus",
            callback: state => manager.IsGameFocused = state
        );
        IsGameHudOpen = CreateBooleanObservable(
            name: "Is Hud open",
            description: "Simulates game Hud state",
            callback: state => manager.IsGameHudVisible = state
        );
        Settings = settings;
    }

    public void NavigateToSettings()
    {
        var settings = Settings.SelectMany(it => _configurationAdapter.Adapt(it, game: GameProcessType.MonsterHunterWilds))
            .ToObservableCollection();

        var viewModel = new SettingsViewModel(
            configurations: new()
            {
                { GameProcessType.MonsterHunterWilds, settings }
            },
            configurableGames: new ObservableCollection<GameProcessType> { GameProcessType.MonsterHunterWilds },
            currentConfiguredGame: GameProcessType.MonsterHunterWilds,
            connector: _poogieVersionConnector
        );

        _bodyNavigator.Navigate(viewModel);
    }

    private static BooleanPropertyViewModel CreateBooleanObservable(
        string name,
        string description,
        Action<bool> callback)
    {
        Observable<bool> observable = new(false);
        observable.PropertyChanged += (_, __) => callback(observable.Value);

        return new BooleanPropertyViewModel(observable)
        {
            Name = name,
            Description = description,
            Group = "",
            RequiresRestart = false,
            Conditions = Array.Empty<PropertyCondition>(),
            IsMatch = true
        };
    }
}