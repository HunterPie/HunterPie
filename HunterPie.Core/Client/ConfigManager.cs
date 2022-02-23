using HunterPie.Core.Extensions;
using HunterPie.Core.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HunterPie.Core.Client
{
    public class ConfigManager
    {
        private readonly static FileSystemWatcher _fileSystemWatcher = new()
        {
            Path = ClientInfo.ClientPath,
            Filter = "*.json",
            NotifyFilter = NotifyFilters.LastWrite,
            EnableRaisingEvents = true,
            IncludeSubdirectories = true
        };
        
        private readonly static Dictionary<string, long> _lastWrites = new Dictionary<string, long>();
        private const long MinTicks = 100 * TimeSpan.TicksPerMillisecond;
        private readonly static Dictionary<string, object> _settings = new Dictionary<string, object>();

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
            path = GetFullPath(path);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(Path.GetDirectoryName(path));

            if (_settings.ContainsKey(path))
                return;

            _settings[path] = @default;
            Reload(path);
        }

        internal static void Initialize()
        {
            Action<string> reloadSetting = (string path) => Reload(path);
            var debounceReload = reloadSetting.Debounce(100);

            _fileSystemWatcher.Changed += (_, args) =>
            {
                if (!Settings.ContainsKey(args.FullPath))
                    return;

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
            path = GetFullPath(path);

            if (!Settings.ContainsKey(path))
            {
                Log.Warn($"'{path}' not registered in ConfigManager.");
                return;
            }

            if (!File.Exists(path))
            {
                string fileName = Path.GetFileName(path);
                Log.Error($"{fileName} is missing. Creating a new one.");

                WriteSettings(path);
            }

            ReadSettings(path);
        }

        public static void Save(string path)
        {
            path = GetFullPath(path);

            if (!Settings.ContainsKey(path))
            {
                Log.Warn($"'{path}' not registered in ConfigManager.");
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
            {
                try
                {
                    using (FileStream stream = File.OpenRead(path))
                    {
                        byte[] buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, buffer.Length);

                        string str = Encoding.UTF8.GetString(buffer);

                        if (string.IsNullOrEmpty(str)
                            || str[0] == '\x00'
                            || str == "null")
                            throw new Exception("Configuration file was empty");

                        var serializerSettings = new JsonSerializerSettings()
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            TypeNameHandling = TypeNameHandling.Auto
                        };

                        JsonConvert.PopulateObject(str, _settings[path], serializerSettings);
                    }
                } catch (Exception err) { Log.Error(err.ToString()); }
            }
        }

        private static void WriteSettings(string path)
        {
            lock (_settings[path])
            {
                try
                {
                    var serializerSettings = new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    };

                    string serialized = JsonConvert.SerializeObject(_settings[path], Formatting.Indented, serializerSettings);
                    ReadOnlySpan<byte> buffer = Encoding.UTF8.GetBytes(serialized);
                    using (FileStream stream = File.OpenWrite(path))
                    {
                        stream.SetLength(0);
                        stream.Write(buffer);
                    }
                } catch(Exception err) { Log.Error(err.ToString()); }
            }
        }

        private static string GetFullPath(string path)
        {
            if (!Path.IsPathFullyQualified(path))
            {
                path = Path.Combine(ClientInfo.ClientPath, path);
            }

            return path;
        }
    }
}
