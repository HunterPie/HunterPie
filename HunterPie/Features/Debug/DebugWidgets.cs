using System;
using System.Linq;

namespace HunterPie.Features.Debug;

internal static class DebugWidgets
{
    private static readonly Lazy<IWidgetMocker[]> mockers = new(() =>
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(asm => asm.GetTypes())
            .Where(types => typeof(IWidgetMocker).IsAssignableFrom(types) && !types.IsInterface)
            .Select(type => Activator.CreateInstance(type))
            .Cast<IWidgetMocker>()
            .ToArray();
    });

    public static void MockIfNeeded()
    {
        foreach (IWidgetMocker mocker in mockers.Value)
            mocker.Mock();
    }
}
