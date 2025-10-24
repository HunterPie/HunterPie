using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Versions;
using HunterPie.Core.Game;
using HunterPie.Core.Input;
using HunterPie.Core.Observability.Logging;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.ViewModels;
using HunterPie.UI.Overlay.Views;
using System;
using System.Collections.Generic;
using System.Windows.Threading;
using ClientConfig = HunterPie.Core.Client.ClientConfig;

namespace HunterPie.Features.Overlay.Services;

internal class OverlayManager : Bindable, IOverlay, IOverlayState, IDisposable
{
    private readonly ILogger _logger = LoggerFactory.Create();
    private readonly LinkedList<WidgetView> _widgets = new();

    private readonly Dispatcher _dispatcher;
    private readonly IHotkeyService _hotkeyService;
    private readonly V5Config _config;

    private bool _isDesignModeEnabled;
    public bool IsDesignModeEnabled
    {
        get => _isDesignModeEnabled;
        internal set
        {
            SetValue(ref _isDesignModeEnabled, value);

            foreach (WidgetView widget in _widgets)
                widget.UpdateFlags();
        }
    }

    private bool _isGameHudVisible;
    public bool IsGameHudVisible { get => _isGameHudVisible; internal set => SetValue(ref _isGameHudVisible, value); }

    private bool _isGameFocused;
    public bool IsGameFocused { get => _isGameFocused; internal set => SetValue(ref _isGameFocused, value); }

    public OverlayManager(
        Dispatcher dispatcher,
        IHotkeyService hotkeyService,
        V5Config config)
    {
        _dispatcher = dispatcher;
        _hotkeyService = hotkeyService;
        _config = config;
    }

    public void Setup(IContext context)
    {
        _hotkeyService.Register(_config.Overlay.ToggleDesignMode, () => IsDesignModeEnabled = !IsDesignModeEnabled);
        _hotkeyService.Register(_config.Overlay.ToggleVisibility, () => _config.Overlay.IsEnabled.Value = !_config.Overlay.IsEnabled);

        context.Process.Focus += (_, __) => IsGameFocused = true;
        context.Process.Blur += (_, __) => IsGameFocused = false;
        context.Game.OnHudStateChange += (_, e) => IsGameHudVisible = e.IsHudOpen;
    }

    public void Dispose()
    {
        _widgets.Clear();
    }

    public WidgetView Register(WidgetViewModel viewModel)
    {
        WidgetView widget = _dispatcher.Invoke(() => new WidgetView
        {
            DataContext = new WidgetContext(
                viewModel: viewModel,
                overlaySettings: ClientConfig.Config.Overlay,
                state: this
            )
        }, DispatcherPriority.Send);

        _widgets.AddLast(widget);

        _logger.Debug($"Registered overlay widget {viewModel.Title} ({viewModel.GetType().Name})");

        widget.Show();

        return widget;
    }

    public void Unregister(WidgetView? widget)
    {
        if (widget is null)
            return;

        widget.Close();

        _widgets.Remove(widget);

        _logger.Debug($"Removed overlay widget {(widget.DataContext as WidgetContext)?.ViewModel.Title}");
    }
}