using HunterPie.Core.Logger;
using HunterPie.Domain.Interfaces;
using HunterPie.Internal.Initializers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace HunterPie.Internal;

internal class InitializerManager
{
    private static readonly HashSet<IInitializer> _initializers = new()
    {
        new CustomFontsInitializer(),
        
        // Core
        new LocalConfigInitializer(),
        new ClientConfigMigrationInitializer(),
        new ClientConfigInitializer(),
        new ConfigManagerInitializer(),
        new FeatureFlagsInitializer(),
        new NativeLoggerInitializer(),
        new HunterPieLoggerInitializer(),
        new MapperFactoryInitializer(),
        new CustomThemeInitializer(),

        new ExceptionCatcherInitializer(),
        new DialogManagerInitializer(),
        new UITracerInitializer(),
        new ClientLocalizationInitializer(),
        new SystemTrayInitializer(),
        new ClientConfigBindingsInitializer(),
        new CredentialVaultInitializer(),
        
        // GUI
        new MenuInitializer(),
    };

    private static readonly HashSet<IInitializer> _uiInitializers = new()
    {
        new HotkeyInitializer(),

        // Debugging
        new DebugWidgetInitializer(),
    };

    public static async Task Initialize()
    {
        Log.Benchmark();

        foreach (IInitializer initializer in _initializers)
            await initializer.Init();

        Log.BenchmarkEnd();
    }

    public static void InitializeGUI()
    {
        Log.Benchmark();

        // Make sure to run UI initializers in the main thread
        Dispatcher.CurrentDispatcher.Invoke(async () =>
        {
            foreach (IInitializer initializer in _uiInitializers)
                await initializer.Init();
        });

        Log.BenchmarkEnd();
    }

    public static void Unload()
    {
        foreach (IInitializer initializer in _initializers)
        {
            if (initializer is IDisposable disposable)
                disposable.Dispose();
        }

        foreach (IInitializer initializer in _uiInitializers)
        {
            if (initializer is IDisposable disposable)
                disposable.Dispose();
        }
    }
}
