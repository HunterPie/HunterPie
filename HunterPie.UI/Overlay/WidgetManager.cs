using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Input;
using HunterPie.Core.Logger;
using HunterPie.Core.Settings;
using HunterPie.UI.Overlay.Components;
using System.Collections.ObjectModel;
using System.Linq;
using ClientConfig = HunterPie.Core.Client.ClientConfig;

namespace HunterPie.UI.Overlay
{
    public class WidgetManager : Bindable
    {
        private bool _isDesignModeEnabled;
        private bool _isGameFocused;
        private readonly ObservableCollection<WidgetBase> _widgets = new ObservableCollection<WidgetBase>();

        public bool IsDesignModeEnabled { get => _isDesignModeEnabled; private set { SetValue(ref _isDesignModeEnabled, value); } }
        public bool IsGameFocused { get => _isGameFocused; internal set { SetValue(ref _isGameFocused, value); } }

        public ref readonly ObservableCollection<WidgetBase> Widgets => ref _widgets;
        public OverlayConfig Settings => ClientConfig.Config.Overlay;

        private static WidgetManager _instance;

        public static WidgetManager Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new WidgetManager();

                return _instance;
            }
        }

        private WidgetManager()
        {
            Hotkey.Register(Settings.ToggleDesignMode, ToggleDesignMode);
        }

        public static bool Register<T, K>(T widget) where T : IWidgetWindow, IWidget<K>
                                                    where K : IWidgetSettings
        {

            WidgetBase wnd = new WidgetBase() { Widget = widget };
            Instance._widgets.Add(wnd);
            wnd.Show();
            
            Log.Debug($"Added new widget: {widget.Title}");

            return true;
        }

        public static bool Unregister<T, K>(T widget) where T : IWidgetWindow, IWidget<K>
                                                      where K : IWidgetSettings
        {
            WidgetBase wnd = Instance._widgets.ToArray()
                .First(wnd => wnd.Widget == (IWidgetWindow)widget);
            
            wnd.Close();
            
            return Instance._widgets.Remove(wnd);
        }

        internal static void Dispose()
        {
            foreach (WidgetBase widget in Instance._widgets)
                widget.Close();

            Instance._widgets.Clear();
        }

        private void ToggleDesignMode()
        {
            IsDesignModeEnabled = !IsDesignModeEnabled;

            foreach (WidgetBase widget in Widgets)
                widget.HandleTransparencyFlag(!IsDesignModeEnabled);
        }
    }
}
