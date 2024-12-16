using HunterPie.Core.Game;
using HunterPie.Core.Logger;
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
            .Select(Activator.CreateInstance)
            .Cast<IWidgetInitializer>()
            .ToArray();
    });

    public static void Initialize(Context context)
    {
        foreach (IWidgetInitializer initializer in Initializers.Value)
            try
            {
                initializer.Load(context);
            }
            catch (Exception err)
            {
                Log.Error(err.ToString());
            }
    }

    public static void Unload()
    {
        foreach (IWidgetInitializer initializer in Initializers.Value)
            initializer.Unload();
    }
}