using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.Widgets.Abnormality.ViewModel;


namespace HunterPie.UI.Overlay.Widgets.Abnormality.View
{
    /// <summary>
    /// Interaction logic for AbnormalityBarView.xaml
    /// </summary>
    public partial class AbnormalityBarView : View<AbnormalityBarViewModel>, IWidget<AbnormalityWidgetConfig>, IWidgetWindow
    {
        private readonly AbnormalityWidgetConfig _config;

        public AbnormalityBarView(ref AbnormalityWidgetConfig config)
        {
            _config = config;
            InitializeComponent();
        }

        public string Title => Settings.Name;
        public AbnormalityWidgetConfig Settings => _config; //ClientConfig.Config.Overlay.AbnormalityTray.Trays.Trays.ElementAtOrDefault(Index);
        public WidgetType Type => WidgetType.ClickThrough;
    }
}
