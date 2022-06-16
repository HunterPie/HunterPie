using HunterPie.Core.Game;
using HunterPie.UI.Architecture.Overlay;

namespace HunterPie.Features.Overlay
{
    internal static class WidgetInitializers
    {
        private static IWidgetInitializer[] Initializers =
        {
            new MonsterWidgetInitializer(),
            new AbnormalitiesWidgetInitializer(),
            new WirebugWidgetInitializer(),
            new ActivitiesWidgetInitializer(),
            new ChatWidgetInitializer(),
            new DamageWidgetInitializer(),
            new SpecializedToolWidgetInitializer(),
        };

        public static void Initialize(Context context)
        {
            foreach (var initializer in Initializers)
                initializer.Load(context);
        }

        public static void Unload()
        {
            foreach (var initializer in Initializers)
                initializer.Unload();
        }
    }
}
