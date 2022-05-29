using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.Widgets.SpecializedTools.ViewModels;
using System;

namespace HunterPie.UI.Overlay.Widgets.SpecializedTools.Views
{
    /// <summary>
    /// Interaction logic for SpecializedToolView.xaml
    /// </summary>
    public partial class SpecializedToolView : View<SpecializedToolViewModel>, IWidget<SpecializedToolWidgetConfig>, IWidgetWindow
    {
        private readonly SpecializedToolWidgetConfig _config;

        public SpecializedToolView(SpecializedToolWidgetConfig config)
        {
            _config = config;
            InitializeComponent();
        }

        public SpecializedToolWidgetConfig Settings => _config;

        public string Title => "Specialized Tool Widget";

        public WidgetType Type => WidgetType.ClickThrough;

        IWidgetSettings IWidgetWindow.Settings => Settings;

        public event EventHandler<WidgetType> OnWidgetTypeChange;
    }
}
