using HunterPie.Core.Logger;
using HunterPie.Domain.Interfaces;
using HunterPie.Internal.Intializers;
using System;
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
            new ClientLocalizationInitializer(),
            new ConfigManagerInitializer(),
            new SystemTrayInitializer(),

            // GUI
            new MenuInitializer(),
        };

        private static HashSet<IInitializer> _uiInitializers = new()
        {
            // Debugging
            new DebugWidgetInitializer(),

            new HotkeyInitializer(),
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

            foreach (IInitializer initializer in _uiInitializers)
                initializer.Init();

            Log.BenchmarkEnd();
        }

        public static void Unload()
        {
            foreach (IInitializer initializer in _initializers)
                if (initializer is IDisposable disposable)
                    disposable.Dispose();

            foreach (IInitializer initializer in _uiInitializers)
                if (initializer is IDisposable disposable)
                    disposable.Dispose();
        }
    }
}
