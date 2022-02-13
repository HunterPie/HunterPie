using HunterPie.Core.Logger;
using System;
using System.Collections.Generic;
using System.Threading;

namespace HunterPie.UI.Architecture.Test
{
    public static class MockBehavior
    {
        private static readonly List<Timer> timers = new();

        public static void Run(Action runnable)
        {
            // Yes, this is a memory leak, but will only be used for mocking purposes anyways
            var timer = new Timer((_) => runnable(), null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1));
            timers.Add(timer);
        }
    }
}
