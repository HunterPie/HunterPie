using HunterPie.Core.Logger;
using HunterPie.Domain.Interfaces;
using HunterPie.Internal.Intializers;
using System.Collections.Generic;

namespace HunterPie.Internal
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

        private static HashSet<IInitializer> _overlayInitializers = new()
        {
            // Debugging
            new DebugWidgetInitializer(),
        };

        public static void Initialize()
        {
            Log.Benchmark();
            
            foreach (IInitializer initializer in _initializers)
                initializer.Init();

            Log.BenchmarkEnd();
        } 

        public static void InitializeGUI()
        {
            Log.Benchmark();

            foreach (IInitializer initializer in _overlayInitializers)
                initializer.Init();

            Log.BenchmarkEnd();
        }
    }
}
