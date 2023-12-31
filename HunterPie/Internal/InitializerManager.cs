using HunterPie.Core.Logger;
using HunterPie.Domain.Interfaces;
using HunterPie.Internal.Initializers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace HunterPie.Internal;

internal class InitializerManager
{
    private static readonly HashSet<IInitializer> Initializers = new()
    {
        new FileStreamLoggerInitializer(),
        new CustomFontsInitializer(),
        
        // Core
        new CredentialVaultInitializer(),
        new LocalConfigInitializer(),
        
        // Feature Flags
        new FeatureFlagsInitializer(),

        // Config
        new RemoteConfigSyncInitializer(),
        new ClientConfigMigrationInitializer(),
        new ClientConfigInitializer(),
        new ConfigManagerInitializer(),

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

        // GUI
        new NavigationTemplatesInitializer(),
        new AppNotificationsInitializer(),
    };

    private static readonly HashSet<IInitializer> UiInitializers = new()
    {
        new HotkeyInitializer(),

        // Debugging
        new DebugWidgetInitializer(),
    };

    public static async Task Initialize()
    {
        Log.Benchmark();

        foreach (IInitializer initializer in Initializers)
            await initializer.Init();

        Log.BenchmarkEnd();
    }

    public static void InitializeGUI()
    {
        Log.Benchmark();

        // Make sure to run UI initializers in the main thread
        Application.Current.Dispatcher.Invoke(async () =>
        {
            foreach (IInitializer initializer in UiInitializers)
                await initializer.Init();
        });

        Log.BenchmarkEnd();
    }

    public static void Unload()
    {
        foreach (IInitializer initializer in Initializers)
            if (initializer is IDisposable disposable)
                disposable.Dispose();

        foreach (IInitializer initializer in UiInitializers)
            if (initializer is IDisposable disposable)
                disposable.Dispose();
    }
}
