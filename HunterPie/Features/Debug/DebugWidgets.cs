namespace HunterPie.Features.Debug;

internal static class DebugWidgets
{
    private static readonly IWidgetMocker[] mockers = new IWidgetMocker[]
    {
        new MonsterWidgetMocker(),
        new AbnormalityWidgetMocker(),
        new ActivitiesWidgetMocker(),
        new DamageWidgetMocker(),
        new WirebugWidgetMocker(),
        new ChatWidgetMocker(),
        new SpecializedToolWidgetMocker(),
        new PlayerHUDWidgetMocker(),
    };

    public static void MockIfNeeded()
    {
        foreach (IWidgetMocker mocker in mockers)
            mocker.Mock();
    }
}
