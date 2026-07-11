using HunterPie.Core.Client.Events;
using HunterPie.Core.Client.Observer;
using HunterPie.Core.Crypto;
using HunterPie.Core.Extensions;
using HunterPie.Core.Json;
using HunterPie.Core.Observability.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace HunterPie.Core.Client;

public class ConfigManager
{
    private static readonly ILogger Logger = LoggerFactory.Create();

    private static readonly FileSystemWatcher _fileSystemWatcher = new()
    {
        Path = ClientInfo.ClientPath,
        Filter = "*.json",
        NotifyFilter = NotifyFilters.LastWrite,
        EnableRaisingEvents = true,
        IncludeSubdirectories = true
    };

    private static readonly Dictionary<string, long> _lastWrites = new();
    private const long MinTicks = 100 * TimeSpan.TicksPerMillisecond;
    private static readonly Dictionary<string, object> _settings = new();
    private static readonly Dictionary<string, string> _hashes = new();

    public static event EventHandler<ConfigSaveEventArgs> OnSync;

    public static IReadOnlyDictionary<string, object> Settings => _settings;

    /// <summary>
    /// Registers a new configuration file for HunterPie to keep track of.
    /// Configuration files MUST be in the json format and also have their file names
    /// end in .json
    /// </summary>
    /// <param name="path">Absolute or relative path for the configuration file</param>
    /// <param name="default">Base class for the config to be serialized to</param>
    public static void Register(string path, object @default)
    {
        path = ConfigHelper.GetFullPath(path);

        if (!Directory.Exists(path))
            _ = Directory.CreateDirectory(Path.GetDirectoryName(path));

        if (_settings.ContainsKey(path))
            return;

        _settings[path] = @default;
        Reload(path);
    }

    internal static void Initialize()
    {
        Action<string> reloadSetting = Reload;
        Action<string> debounceReload = reloadSetting.Debounce(200);

        _fileSystemWatcher.Changed += (_, args) =>
        {
            if (!Settings.ContainsKey(args.FullPath))
                return;

            string cachedHash = string.Empty;
            if (_hashes.ContainsKey(args.FullPath))
                cachedHash = _hashes[args.FullPath];

            string currentHash = HashService.Hash(
                value: ConfigHelper.ReadObject(args.FullPath)
            );

            if (cachedHash == currentHash)
                return;

            _hashes[args.FullPath] = currentHash;

            long lastWrite = File.GetLastWriteTime(args.FullPath).Ticks;

            if (!_lastWrites.ContainsKey(args.FullPath)
                || (lastWrite - _lastWrites[args.FullPath] > MinTicks))
            {
                _lastWrites[args.FullPath] = lastWrite;

                debounceReload(args.FullPath);
            }
        };
    }

    public static void Reload(string path)
    {
        path = ConfigHelper.GetFullPath(path);

        if (!Settings.ContainsKey(path))
        {
            Logger.Warning($"'{path}' not registered in ConfigManager.");
            return;
        }

        if (!File.Exists(path))
        {
            string fileName = Path.GetFileName(path);
            Logger.Error($"'{fileName}' not registered in ConfigManager.");

            WriteSettings(path);
        }

        ReadSettings(path);
        OnSync?.Invoke(null, new(path));
    }

    public static void Save(string path)
    {
        path = ConfigHelper.GetFullPath(path);

        if (!Settings.ContainsKey(path))
        {
            Logger.Warning($"'{path}' not registered in ConfigManager.");
            return;
        }

        WriteSettings(path);
    }

    public static void SaveAll()
    {
        foreach (string config in Settings.Keys)
            Save(config);
    }

    private static void ReadSettings(string path)
    {
        lock (_settings[path])
            try
            {
                string str = ConfigHelper.ReadObject(path);

                JsonProvider.Populate(str, _settings[path]);
            }
            catch (Exception err)
            {
                Logger.Error(err.ToString());
            }
    }

    private static void WriteSettings(string path)
    {
        lock (_settings[path])
            ConfigHelper.WriteObject(path, _settings[path]);
    }

    public static void BindConfiguration(string path, object data)
    {
        ConfigurationBinder.Bind(data, () => Save(path));
    }
}