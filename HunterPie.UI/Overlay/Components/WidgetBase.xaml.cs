using HunterPie.UI.Architecture.Extensions;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interop;
using System;
using HunterPie.UI.Platform.Windows.Native;
using HunterPie.UI.Overlay.Enums;
using System.Windows.Media;
using System.Windows.Threading;
using HunterPie.Core.Logger;
using System.Windows.Controls;

namespace HunterPie.UI.Overlay.Components
{
    /// <summary>
    /// Interaction logic for WidgetBase.xaml
    /// </summary>
    public partial class WidgetBase : Window, INotifyPropertyChanged
    {
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

        private UserControl _widget;
        public UserControl Widget
        {
            get => _widget;
            internal set
            {
                if (value != _widget)
                {
                    _widget = value;
                    this.N(PropertyChanged);
                }
            }
        }

        public WidgetBase()
        {
            InitializeComponent();
            DataContext = this;
            CompositionTarget.Rendering += OnRender;
        }

        private int counter = 0;
        private void OnRender(object sender, EventArgs e)
        {
            if (counter >= 60)
            {
                ForceAlwaysOnTop();
                // Force WPF to update all its bindings because for some reason some people have issues
                // with bindings taking too long to be updated.
                Widget.InvalidateVisual();
                Dispatcher.Invoke(() => { }, DispatcherPriority.Render);
                counter = 0;
            }
            counter++;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            CompositionTarget.Rendering -= OnRender;

            base.OnClosing(e);
        }

        private void SetWindowFlags()
        {
            IntPtr hWnd = new WindowInteropHelper(this)
                .EnsureHandle();

            uint styles = (uint)User32.GetWindowLong(hWnd, User32.GWL_EXSTYLE);

            uint flags = ((IWidgetWindow)Widget).Type switch
            {
                WidgetType.ClickThrough => ClickThroughFlags,
                WidgetType.Window => WindowFlags,
                _ => throw new NotImplementedException("Unreachable"),
            };

            User32.SetWindowLong(hWnd, User32.GWL_EXSTYLE, (int)(styles | flags));
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            SetWindowFlags();
        }

        internal void HandleTransparencyFlag(bool enableFlag)
        {
            IntPtr hWnd = new WindowInteropHelper(this)
                .EnsureHandle();

            uint styles = (uint)User32.GetWindowLong(hWnd, User32.GWL_EXSTYLE);

            if (enableFlag)
                styles |= (uint)(User32.EX_WINDOW_STYLES.WS_EX_TRANSPARENT);
            else
                styles &= ~(uint)(User32.EX_WINDOW_STYLES.WS_EX_TRANSPARENT);

            User32.SetWindowLong(hWnd, User32.GWL_EXSTYLE, (int)styles);
        }

        private void ForceAlwaysOnTop()
        {
            IntPtr hWnd = new WindowInteropHelper(this)
                .EnsureHandle();

            User32.SetWindowPos(hWnd, -1, 0, 0, 0, 0, Flags);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
