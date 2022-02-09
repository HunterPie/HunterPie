using HunterPie.Core.Client;
using HunterPie.Core.Input;
using HunterPie.Core.Logger;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture.Extensions;
using HunterPie.UI.Overlay.Components;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;

namespace HunterPie.UI.Overlay
{
    public class WidgetManager : INotifyPropertyChanged
    {
        private bool _isDesignModeEnabled;
        private readonly ObservableCollection<WidgetBase> _widgets = new ObservableCollection<WidgetBase>();

        public bool IsDesignModeEnabled
        {
            get => _isDesignModeEnabled;
            private set
            {
                if (value != _isDesignModeEnabled)
                {
                    _isDesignModeEnabled = value;
                    this.N(PropertyChanged);
                }
            }
        }
        public ref readonly ObservableCollection<WidgetBase> Widgets => ref _widgets;

        private static WidgetManager _instance;

        public event PropertyChangedEventHandler PropertyChanged;
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
            Hotkey.Register(ClientConfig.Config.Overlay.ToggleDesignMode, EnterDesignMode);
        }

        public static bool Register<T>(IWidget<T> widget) where T : IWidgetSettings
        {

            WidgetBase wnd = new WidgetBase() { Widget = (UserControl)widget };
            Instance._widgets.Add(wnd);
            wnd.Show();
            
            Log.Debug($"Added new widget: {widget.Title}");

            return true;
        }

        internal static void Dispose()
        {
            foreach (WidgetBase widget in Instance._widgets)
                widget.Close();

            Instance._widgets.Clear();
        }

        private void EnterDesignMode()
        {
            IsDesignModeEnabled = !IsDesignModeEnabled;

            foreach (WidgetBase widget in Widgets)
                widget.HandleTransparencyFlag(!IsDesignModeEnabled);
        }
    }
}
