using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Game;
using HunterPie.Core.Input;
using HunterPie.Core.Observability.Logging;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.ViewModels;
using HunterPie.UI.Overlay.Views;
using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace HunterPie.Features.Overlay.Services;

internal class OverlayManager(
    IContext context,
    Dispatcher dispatcher,
    IHotkeyService hotkeyService,
    IConfiguration config
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

    public WidgetView Register(WidgetViewModel viewModel) => dispatcher.Invoke(() =>
    {
        var widget = new WidgetView
        {
            DataContext = new WidgetContext(
                viewModel: viewModel,
                overlaySettings: config.Overlay,
                developmentSettings: config.Development,
                state: this
            )
        };

        _widgets.AddLast(widget);

        _logger.Debug($"Registered overlay widget {viewModel.Title} ({viewModel.GetType().Name})");

        widget.Show();

        return widget;
    }, DispatcherPriority.Send);

    public void Unregister(WidgetView? widget) => dispatcher.Invoke(() =>
    {
        if (widget is null)
            return;

        widget.Close();

        _widgets.Remove(widget);

        _logger.Debug($"Removed overlay widget {(widget.DataContext as WidgetContext)?.ViewModel.Title}");
    }, DispatcherPriority.Send);

}