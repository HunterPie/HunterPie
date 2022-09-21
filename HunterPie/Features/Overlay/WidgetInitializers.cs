using HunterPie.Core.Game;
using HunterPie.UI.Architecture.Overlay;

namespace HunterPie.Features.Overlay;

internal static class WidgetInitializers
{
    private static readonly IWidgetInitializer[] Initializers =
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
        foreach (IWidgetInitializer initializer in Initializers)
            initializer.Load(context);
    }

    public static void Unload()
    {
        foreach (IWidgetInitializer initializer in Initializers)
            initializer.Unload();
    }
}
