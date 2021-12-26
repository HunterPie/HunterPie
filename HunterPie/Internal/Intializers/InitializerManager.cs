using HunterPie.Core.Logger;
using HunterPie.Domain.Interfaces;
using System.Collections.Generic;

namespace HunterPie.Internal.Intializers
{
    internal class InitializerManager
    {
        private static HashSet<IInitializer> _initializers = new()
        {
            // Core
            new HunterPieLoggerInitializer(),
            new FeatureFlagsInitializer(),

            new NativeLoggerInitializer(),
            new ExceptionCatcherInitializer(),
            new DialogManagerInitializer(),
            new UITracerInitializer(),
            new ClientConfigInitializer(),
            new ConfigManagerInitializer(),

            // GUI
            new MenuInitializer(),
        };

        public static void Initialize()
        {
            Log.Benchmark();
            
            foreach (IInitializer initializer in _initializers)
                initializer.Init();

            Log.BenchmarkEnd();
        } 
    }
}
