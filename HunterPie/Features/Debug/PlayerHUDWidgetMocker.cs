namespace HunterPie.Features.Debug;
internal class PlayerHudWidgetMocker : IWidgetMocker
{
    public void Mock()
    {
        return;
        /*_ = WidgetManager.Register<PlayerHudView, PlayerHudWidgetConfig>(
                new PlayerHudView()
                {
                    DataContext = new MockPlayerHudViewModel()
                }
            );*/
    }
}
