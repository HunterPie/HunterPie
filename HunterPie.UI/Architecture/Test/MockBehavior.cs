using System;
using System.Windows.Threading;

namespace HunterPie.UI.Architecture.Test;

public static class MockBehavior
{
    public static DispatcherTimer Run(Action runnable, float seconds = 1)
    {
        return new DispatcherTimer(
            interval: TimeSpan.FromSeconds(seconds),
            priority: DispatcherPriority.Render,
            callback: (_, _) => runnable(),
            dispatcher: Dispatcher.CurrentDispatcher
        );
    }
}