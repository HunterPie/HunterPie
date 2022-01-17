using HunterPie.UI.System.Windows;
using HunterPie.UI.Architecture.Extensions;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interop;
using System;
using HunterPie.UI.Overlay.Enums;

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
            | User32.EX_WINDOW_STYLES.WS_EX_NOACTIVATE);

        private const uint WindowFlags =
            (uint)(User32.EX_WINDOW_STYLES.WS_EX_TOPMOST
            | User32.EX_WINDOW_STYLES.WS_EX_NOACTIVATE);

        private object _widget;
        
        public object Widget
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
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

            // TODO: Streamer mode make remove this flag
            flags |= (uint)User32.EX_WINDOW_STYLES.WS_EX_TOOLWINDOW;

            User32.SetWindowLong(hWnd, User32.GWL_EXSTYLE, (int)(styles | flags));
        }

        private void OnLoaded(Object sender, RoutedEventArgs e)
        {
            SetWindowFlags();
        }
    }
}
