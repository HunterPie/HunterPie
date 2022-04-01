using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using System;

namespace HunterPie.UI.Overlay.Widgets.Monster.Views
{
    /// <summary>
    /// Interaction logic for MonstersView.xaml
    /// </summary>
    public partial class MonstersView : View<MonstersViewModel>, IWidget<MonsterWidgetConfig>, IWidgetWindow
    {
        public MonstersView()
        {
            InitializeComponent();
        }

        public MonsterWidgetConfig Settings => ClientConfig.Config.Overlay.BossesWidget;
        public string Title => "Monsters Widget";
        public WidgetType Type => WidgetType.ClickThrough;
        IWidgetSettings IWidgetWindow.Settings => Settings;

        public event EventHandler<WidgetType> OnWidgetTypeChange;
    }
}
