using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Game;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.ViewModels;
using HunterPie.UI.Overlay.Views;
using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace HunterPie.Features.Overlay.Services;

internal class OverlayManager : Bindable, IOverlay, IOverlayState, IDisposable
{
    private readonly Dispatcher _dispatcher;
    private readonly LinkedList<WidgetView> _widgets = new();

    private WeakReference<IContext>? _context;

    private bool _isDesignModeEnabled;
    public bool IsDesignModeEnabled { get => _isDesignModeEnabled; private set => SetValue(ref _isDesignModeEnabled, value); }

    private bool _isGameHudVisible;
    public bool IsGameHudVisible { get => _isGameHudVisible; private set => SetValue(ref _isGameHudVisible, value); }

    private bool _isGameFocused;
    public bool IsGameFocused { get => _isGameFocused; private set => SetValue(ref _isGameFocused, value); }

    public OverlayManager(Dispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public void Setup(IContext context)
    {
        _context = new WeakReference<IContext>(context);
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
        if (_context is null || !_context.TryGetTarget(out IContext? ctx))
            throw new NullReferenceException("Cannot register a widget when the game context is null");

        OverlayConfig settings = ClientConfigHelper.GetOverlayConfigFrom(ctx.Process.Type);

        WidgetView widget = _dispatcher.Invoke(() => new WidgetView
        {
            DataContext = new WidgetContext(
                viewModel: viewModel,
                overlaySettings: settings,
                state: this
            )
        }, DispatcherPriority.Send);

        _widgets.AddLast(widget);

        return widget;
    }

    public void Unregister(WidgetView widget)
    {
        _widgets.Remove(widget);
    }
}