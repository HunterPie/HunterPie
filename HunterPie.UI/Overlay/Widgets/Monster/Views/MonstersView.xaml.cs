using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;

namespace HunterPie.UI.Overlay.Widgets.Monster.Views
{
    /// <summary>
    /// Interaction logic for MonstersView.xaml
    /// </summary>
    public partial class MonstersView : View<MonstersViewModel>, IWidget<MonsterWidgetConfig>
    {
        public MonstersView()
        {
            InitializeComponent();
        }

        public MonsterWidgetConfig Settings => ClientConfig.Config.Overlay.EndemicWidget;

        public string Title => "Monsters Widget";
    }
}
