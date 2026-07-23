using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration;
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

internal class WidgetMocksProvider(
    Dispatcher dispatcher,
    IWidgetMocker[] mockers,
    IConfiguration config,
    ConfigurationAdapter configurationAdapter,
    PoogieVersionConnector poogieVersionConnector,
    IBodyNavigator bodyNavigator
)
{
    private readonly OverlayManager _overlay = new OverlayManager(null, dispatcher, new HotkeyServiceMock(), config);
    private readonly Dictionary<IWidgetMocker, WidgetView> _views = new();
    private readonly ObservableCollection<IWidgetSettings> _settings = new();

    public void MockEnabled()
    {
        foreach (IWidgetMocker mocker in mockers)
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

        AttachAndRun(config.Development.IsOverlayManagerDebugEnabled, (enabled) =>
        {
            dispatcher.Invoke(() =>
            {
                if (!enabled)
                    return;

                var view = new DebugOverlayManagerView
                {
                    DataContext = new DebugOverlayManagerViewModel(
                        manager: _overlay,
                        configurationAdapter: configurationAdapter,
                        poogieVersionConnector: poogieVersionConnector,
                        bodyNavigator: bodyNavigator,
                        settings: _settings
                    )
                };
                view.Closed += (_, __) => config.Development.IsOverlayManagerDebugEnabled.Value = false;
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