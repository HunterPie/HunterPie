using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.Widgets.Abnormality.ViewModel;
using System;
using System.Windows;
using System.Windows.Media;

namespace HunterPie.UI.Overlay.Widgets.Abnormality.View
{
    /// <summary>
    /// Interaction logic for AbnormalityBarView.xaml
    /// </summary>
    public partial class AbnormalityBarView : View<AbnormalityBarViewModel>, IWidget<AbnormalityWidgetConfig>, IWidgetWindow
    {
        private readonly AbnormalityWidgetConfig _config;
        public string Title => Settings.Name;
        public AbnormalityWidgetConfig Settings => _config;
        public WidgetType Type => WidgetType.ClickThrough;

        IWidgetSettings IWidgetWindow.Settings => _config;

        public AbnormalityBarView(ref AbnormalityWidgetConfig config)
        {
            _config = config;
            InitializeComponent();
            
        }

        int frameCounter = 0;

        public event EventHandler<WidgetType> OnWidgetTypeChange;

        private void OnRender(object sender, EventArgs e)
        {
            // Sort abnormalities every 60 frames
            if (frameCounter >= 60)
            {
                ViewModel.SortAbnormalities(_config.SortByAlgorithm);
                frameCounter = 0;
            }
            frameCounter++;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering += OnRender; 
        }

        private void OnUnload(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering -= OnRender;
        }
    }
}
