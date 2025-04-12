using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using HunterPie.UI.Overlay.Widgets.Monster.Views;
using ClientConfig = HunterPie.Core.Client.ClientConfig;

namespace HunterPie.Features.Debug;

internal class MonsterWidgetMocker : IWidgetMocker
{
    public void Mock()
    {
        OverlayConfig mockConfig = ClientConfig.Config.Rise.Overlay;

        if (!ClientConfig.Config.Development.MockBossesWidget)
            return;

        _ = WidgetManager.Register<MonstersView, MonsterWidgetConfig>(
            new MonstersView(mockConfig.BossesWidget)
            {
                DataContext = new MockMonstersViewModel(mockConfig.BossesWidget)
            }
        );
    }
}