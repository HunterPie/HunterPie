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

internal class OverlayManager(
    IContext context,
    Dispatcher dispatcher,
    IHotkeyService hotkeyService,
    V5Config config
) : Bindable, IOverlay, IOverlayState, IDisposable
{
    private readonly ILogger _logger = LoggerFactory.Create();
    private readonly LinkedList<WidgetView> _widgets = new();

    private int _designModeHotkeyId;
    private int _overlayToggleHotkeyId;

    public bool IsDesignModeEnabled
    {
        get;
        internal set
        {
            SetValue(ref field, value);

            foreach (WidgetView widget in _widgets)
                widget.UpdateFlags();
        }
    }

    public bool IsGameHudVisible { get; internal set => SetValue(ref field, value); }
    public bool IsGameFocused { get; internal set => SetValue(ref field, value); }

    public void Setup()
    {
        _designModeHotkeyId = hotkeyService.Register(config.Overlay.ToggleDesignMode, () => IsDesignModeEnabled = !IsDesignModeEnabled);
        _overlayToggleHotkeyId = hotkeyService.Register(config.Overlay.ToggleVisibility, () => config.Overlay.IsEnabled.Value = !config.Overlay.IsEnabled);

        context.Process.Focus += (_, __) => IsGameFocused = true;
        context.Process.Blur += (_, __) => IsGameFocused = false;
        context.Game.OnHudStateChange += (_, e) => IsGameHudVisible = e.IsHudOpen;
    }

    public void Dispose()
    {
        hotkeyService.Unregister(_designModeHotkeyId);
        hotkeyService.Unregister(_overlayToggleHotkeyId);
        _widgets.Clear();
    }

    public WidgetView Register(WidgetViewModel viewModel)
    {
        WidgetView widget = dispatcher.Invoke(() => new WidgetView
        {
            DataContext = new WidgetContext(
                viewModel: viewModel,
                overlaySettings: ClientConfig.Config.Overlay,
                developmentSettings: ClientConfig.Config.Development,
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