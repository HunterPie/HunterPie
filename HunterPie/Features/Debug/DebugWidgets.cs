namespace HunterPie.Features.Debug
{
    internal static class DebugWidgets
    {
        private readonly static IWidgetMocker[] mockers = new IWidgetMocker[]
        {
            new MonsterWidgetMocker(),
            new AbnormalityWidgetMocker(),
            new ActivitiesWidgetMocker(),
            new DamageWidgetMocker(),
            new WirebugWidgetMocker(),
            new ChatWidgetMocker(),
            new SpecializedToolWidgetMocker(),
        };

        public static void MockIfNeeded()
        {
            foreach (var mocker in mockers)
                mocker.Mock();
        }
    }
}
