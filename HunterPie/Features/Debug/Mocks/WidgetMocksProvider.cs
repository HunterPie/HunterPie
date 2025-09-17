using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Versions;
using HunterPie.Features.Debug.Services;
using HunterPie.Features.Debug.ViewModels;
using HunterPie.Features.Debug.Views;
using HunterPie.Features.Overlay.Services;
using HunterPie.UI.Overlay.Views;
using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace HunterPie.Features.Debug.Mocks;

internal class WidgetMocksProvider
{
    private readonly Dispatcher _dispatcher;
    private readonly OverlayManager _overlay;
    private readonly V5Config _config;
    private readonly IWidgetMocker[] _mockers;
    private readonly Dictionary<IWidgetMocker, WidgetView> _views = new();

    public WidgetMocksProvider(
        Dispatcher dispatcher,
        IWidgetMocker[] mockers,
        V5Config config)
    {
        _dispatcher = dispatcher;
        _config = config;
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
                    _views.Add(mocker, mocker.Mock(_overlay));
                    return;
                }

                _overlay.Unregister(_views[mocker]);
                _views.Remove(mocker);
            });

        AttachAndRun(_config.Development.IsOverlayManagerDebugEnabled, (enabled) =>
        {
            _dispatcher.Invoke(() =>
            {
                if (!enabled)
                    return;

                var view = new DebugOverlayManagerView { DataContext = new DebugOverlayManagerViewModel(_overlay) };
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