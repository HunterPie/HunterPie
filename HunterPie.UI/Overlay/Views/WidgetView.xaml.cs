using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.ViewModels;
using HunterPie.UI.Platform.Windows.Native;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace HunterPie.UI.Overlay.Views;

/// <summary>
/// Interaction logic for WidgetView.xaml
/// </summary>
public partial class WidgetView
{
    private nint? _hWnd;
    private readonly object _sync = new();
    private bool _isClosed;
    private DateTime _lastRenderAt;

    private WidgetContext Context => (WidgetContext)DataContext;

    public WidgetView()
    {
        InitializeComponent();

        CompositionTarget.Rendering += OnRender;
    }

    private void OnRender(object sender, EventArgs e)
    {
        DateTime lastRender = _lastRenderAt;
        _lastRenderAt = DateTime.Now;
        TimeSpan timeDiff = lastRender - _lastRenderAt;

        if (timeDiff.TotalMilliseconds > 500)
            ForceAlwaysOnTop();
    }

    protected override void OnClosed(EventArgs e)
    {
        lock (_sync)
        {
            _isClosed = true;
            base.OnClosed(e);
        }
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        CompositionTarget.Rendering -= OnRender;
        base.OnClosing(e);
    }

    private void OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        double step = 0.05 * (e.Delta > 0 ? 1 : -1);

        if (Keyboard.IsKeyDown(Key.LeftCtrl))
            Context.ViewModel.Settings.Opacity.Current += step;
        else
            Context.ViewModel.Settings.Scale.Current += step;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        SetFlags();

        Context.ViewModel.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName != nameof(WidgetViewModel.Type))
                return;

            Dispatcher.BeginInvoke(SetFlags);
        };
    }

    private void SetFlags()
    {
        nint hWnd = GetHandle();

        uint currentStyle = User32.GetWindowLong(hWnd, User32.GWL_EXSTYLE);

        uint flags = Context.ViewModel.Type switch
        {
            WidgetType.ClickThrough => User32.CLICK_THROUGH_FLAGS,
            WidgetType.Window => User32.WINDOW_FLAGS,
            _ => throw new ArgumentOutOfRangeException(nameof(WidgetType))
        };

        User32.SetWindowLong(
            hwnd: hWnd,
            index: User32.GWL_EXSTYLE,
            style: currentStyle | flags
        );
    }

    private void ForceAlwaysOnTop()
    {
        lock (_sync)
            if (_isClosed)
                return;

        if (Context.State.IsDesignModeEnabled)
            return;

        nint hWnd = GetHandle();

        User32.SetWindowPos(
            hWnd: hWnd,
            hWndInsertAfter: -1,
            x: 0,
            y: 0,
            cx: 0,
            cy: 0,
            uFlags: User32.DEFAULT_FLAGS
        );
    }

    private nint GetHandle()
    {
        return _hWnd ??= new WindowInteropHelper(this).EnsureHandle();
    }
}
