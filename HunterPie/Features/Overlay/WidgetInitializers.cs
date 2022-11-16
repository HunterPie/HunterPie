using HunterPie.Core.Game;
using HunterPie.UI.Architecture.Overlay;
using System;
using System.Linq;

namespace HunterPie.Features.Overlay;

internal static class WidgetInitializers
{
    private static readonly Lazy<IWidgetInitializer[]> Initializers = new(() =>
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(asm => asm.GetTypes())
            .Where(types => typeof(IWidgetInitializer).IsAssignableFrom(types) && !types.IsInterface)
            .Select(type => Activator.CreateInstance(type))
            .Cast<IWidgetInitializer>()
            .ToArray();
    });

    public static void Initialize(Context context)
    {
        foreach (IWidgetInitializer initializer in Initializers.Value)
            initializer.Load(context);

    }

    public static void Unload()
    {
        foreach (IWidgetInitializer initializer in Initializers.Value)
            initializer.Unload();
    }
}
