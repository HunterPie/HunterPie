using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Versions;
using HunterPie.Core.Settings;
using HunterPie.Features.Debug.Services;
using HunterPie.Features.Debug.ViewModels;
using HunterPie.Features.Debug.Views;
using HunterPie.Features.Overlay.Services;
using HunterPie.Integrations.Poogie.Version;
using HunterPie.UI.Navigation;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace HunterPie.Features.Debug.Mocks;

internal class WidgetMocksProvider
{
    private readonly Dispatcher _dispatcher;
    private readonly OverlayManager _overlay;
    private readonly V5Config _config;
    private readonly IWidgetMocker[] _mockers;
    private readonly ConfigurationAdapter _configurationAdapter;
    private readonly PoogieVersionConnector _poogieVersionConnector;
    private readonly IBodyNavigator _bodyNavigator;
    private readonly Dictionary<IWidgetMocker, WidgetView> _views = new();
    private readonly ObservableCollection<IWidgetSettings> _settings = new();

    public WidgetMocksProvider(
        Dispatcher dispatcher,
        IWidgetMocker[] mockers,
        V5Config config,
        ConfigurationAdapter configurationAdapter,
        PoogieVersionConnector poogieVersionConnector,
        IBodyNavigator bodyNavigator)
    {
        _dispatcher = dispatcher;
        _config = config;
        _configurationAdapter = configurationAdapter;
        _poogieVersionConnector = poogieVersionConnector;
        _bodyNavigator = bodyNavigator;
        _overlay = new OverlayManager(dispatcher, new HotkeyServiceMock(), config)
        {
            IsGameFocused = true
        };

        _mockers = mockers;
    }

    public void MockEnabled()
    {
        foreach (IWidgetMocker mocker in _mockers)
            AttachAndRun(mocker.Setting, (_) =>
            {
                if (!_views.ContainsKey(mocker))
                {
                    WidgetView widgetView = mocker.Mock(_overlay);
                    _views.Add(mocker, widgetView);
                    _settings.Add(widgetView.Context.ViewModel.Settings);
                    return;
                }

                WidgetView view = _views[mocker];
                _overlay.Unregister(view);
                _views.Remove(mocker);
                _settings.Remove(view.Context.ViewModel.Settings);
            });

        AttachAndRun(_config.Development.IsOverlayManagerDebugEnabled, (enabled) =>
        {
            _dispatcher.Invoke(() =>
            {
                if (!enabled)
                    return;

                var view = new DebugOverlayManagerView
                {
                    DataContext = new DebugOverlayManagerViewModel(
                        manager: _overlay,
                        configurationAdapter: _configurationAdapter,
                        poogieVersionConnector: _poogieVersionConnector,
                        bodyNavigator: _bodyNavigator,
                        settings: _settings
                    )
                };
                view.Closed += (_, __) => _config.Development.IsOverlayManagerDebugEnabled.Value = false;
                view.Show();
            });
        });
    }

    private static void AttachAndRun(Observable<bool> observable, Action<bool> action)
    {
        observable.PropertyChanged += (_, __) => action(observable.Value);

        if (!observable.Value)
            return;

        action(observable.Value);
    }
}