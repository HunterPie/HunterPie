using HunterPie.Core.Client;
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
        private readonly int Index;

        public AbnormalityBarView(int index)
        {
            Index = index;
            InitializeComponent();
        }

        public string Title => "Abnormality Widget";
        public AbnormalityWidgetConfig Settings => ClientConfig.Config.Overlay.AbnormalityTray.Trays[Index];
        public WidgetType Type => WidgetType.ClickThrough;
    }
}
