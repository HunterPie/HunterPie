using HunterPie.Core.Logger;
using HunterPie.UI.Architecture.Extensions;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Platform.Windows.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
#if DEBUG
using LiveCharts;
using LiveCharts.Defaults;
using HunterPie.UI.Architecture.Graphs;
#endif

namespace HunterPie.UI.Overlay.Components;

/// <summary>
/// Interaction logic for WidgetBase.xaml
/// </summary>
public partial class WidgetBase : Window, INotifyPropertyChanged
{
    private DateTime _lastRender;
    private double _renderingTime;

    public double RenderingTime { get => _renderingTime; private set => SetValue(ref _renderingTime, value); }

#if DEBUG
    public SeriesCollection RenderSeries { get; private set; }
    private readonly ChartValues<ObservablePoint> _renderPoints = new();
#endif

    // TODO: Move this to platform dependent classes
    private const uint Flags =
        (uint)(User32.SWP_WINDOWN_FLAGS.SWP_SHOWWINDOW
        | User32.SWP_WINDOWN_FLAGS.SWP_NOMOVE
        | User32.SWP_WINDOWN_FLAGS.SWP_NOSIZE
        | User32.SWP_WINDOWN_FLAGS.SWP_NOACTIVATE);

    private const uint ClickThroughFlags =
        (uint)(User32.EX_WINDOW_STYLES.WS_EX_TRANSPARENT
        | User32.EX_WINDOW_STYLES.WS_EX_TOPMOST
        | User32.EX_WINDOW_STYLES.WS_EX_NOACTIVATE
        | User32.EX_WINDOW_STYLES.WS_EX_TOOLWINDOW);

    private const uint WindowFlags =
        (uint)(User32.EX_WINDOW_STYLES.WS_EX_TOPMOST
        | User32.EX_WINDOW_STYLES.WS_EX_NOACTIVATE
        | User32.EX_WINDOW_STYLES.WS_EX_TOOLWINDOW);

    private IWidgetWindow _widget;
    public IWidgetWindow Widget
    {
        get => _widget;
        init
        {
            if (value != _widget)
            {
                _widget = value;
                _widget.OnWidgetTypeChange += OnWidgetTypeChange;
                this.N(PropertyChanged);
            }
        }
    }

    public WidgetBase()
    {
#if DEBUG
        RenderSeries = new LinearSeriesCollectionBuilder()
            .AddSeries(_renderPoints, "Render", Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF))
            .Build();
#endif

        InitializeComponent();
        DataContext = this;

        CompositionTarget.Rendering += OnRender;
    }

    private int _counter = 0;
    private void OnRender(object sender, EventArgs e)
    {
        if (_counter >= 60)
        {
            RenderingTime = (DateTime.Now - _lastRender).TotalMilliseconds;
            ForceAlwaysOnTop();
            _counter = 0;
        }

        _lastRender = DateTime.Now;
        _counter++;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        CompositionTarget.Rendering -= OnRender;
        Widget.OnWidgetTypeChange -= OnWidgetTypeChange;
        base.OnClosing(e);
    }

    private void SetWindowFlags()
    {
        IntPtr hWnd = new WindowInteropHelper(this)
            .EnsureHandle();

        uint styles = (uint)User32.GetWindowLong(hWnd, User32.GWL_EXSTYLE);

        uint flags = Widget.Type switch
        {
            WidgetType.ClickThrough => ClickThroughFlags,
            WidgetType.Window => WindowFlags,
            _ => throw new NotImplementedException("Unreachable"),
        };

        _ = User32.SetWindowLong(hWnd, User32.GWL_EXSTYLE, (int)(styles | flags));
    }

    private void OnLoaded(object sender, RoutedEventArgs e) => SetWindowFlags();

    internal void HandleTransparencyFlag(bool enableFlag)
    {
        IntPtr hWnd = new WindowInteropHelper(this)
            .EnsureHandle();

        uint styles = (uint)User32.GetWindowLong(hWnd, User32.GWL_EXSTYLE);

        if (enableFlag)
            styles |= (uint)User32.EX_WINDOW_STYLES.WS_EX_TRANSPARENT;
        else
            styles &= ~(uint)User32.EX_WINDOW_STYLES.WS_EX_TRANSPARENT;

        int result = User32.SetWindowLong(hWnd, User32.GWL_EXSTYLE, (int)styles);

        if (result == 0)
            Log.Error("Failed to set widget {0} flags due to error code: {1}", Widget.GetType().Name, Marshal.GetLastWin32Error());
        else
            Log.Debug("Changed widget flags to {0:X}", result);
    }

    private void ForceAlwaysOnTop()
    {
        IntPtr hWnd = new WindowInteropHelper(this)
            .EnsureHandle();

        _ = User32.SetWindowPos(hWnd, -1, 0, 0, 0, 0, Flags);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void SetValue<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(property, value))
            return;

        property = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        double step = 0.01 * (e.Delta > 0 ? 1 : -1);

        if (Keyboard.IsKeyDown(Key.LeftCtrl))
            Widget.Settings.Opacity.Current += step;
        else
            Widget.Settings.Scale.Current += step;
    }
    private void OnWidgetTypeChange(object sender, WidgetType e)
    {
        Dispatcher.Invoke(() =>
        {
            switch (e)
            {
                case WidgetType.ClickThrough: HandleTransparencyFlag(true); break;
                case WidgetType.Window: HandleTransparencyFlag(false); break;
                default: throw new Exception("unreachable");
            }
        });
    }
}
