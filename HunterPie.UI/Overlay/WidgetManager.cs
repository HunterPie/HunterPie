using HunterPie.Core.Logger;
using HunterPie.UI.Architecture.Extensions;
using HunterPie.UI.Overlay.Components;
using System.Collections.ObjectModel;
using System.ComponentModel;

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

        private WidgetManager() { }

        public static bool Register(IWidget widget)
        {
            Instance._widgets.Add(new WidgetBase() { Widget = widget });
            
            Log.Debug($"Added new widget: {widget.Title}");

            return true;
        }
    }
}
