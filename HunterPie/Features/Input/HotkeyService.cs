using HunterPie.Core.Input;
using HunterPie.Core.Observability.Logging;
using HunterPie.Platforms.Windows.Api.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace HunterPie.Features.Input;

internal class HotkeyService : IHotkeyService, IDisposable
{
    private const int WM_HOTKEY = 0x0312;
    private readonly ILogger _logger = LoggerFactory.Create();
    private readonly Dictionary<int, Action> _hotkeys = new();
    private readonly Random _random = new();
    private HwndSource? _source;

    internal void Setup(HwndSource source)
    {
        source.AddHook(HwndHook);
        _source = source;
    }

    public int Register(string hotkey, Action callback)
    {
        int id = _random.Next();

        if (ParseHotkeyFrom(hotkey) is not { } combination)
        {
            _logger.Error($"{hotkey} is not a valid hotkey");
            return -1;
        }

        if (combination is (0, 0))
            return 0;

        bool success = User32.RegisterHotKey(
            hWnd: _source!.Handle,
            id: id,
            fsModifiers: combination.Item1,
            vlc: combination.Item2
        );

        if (success)
        {
            _hotkeys[id] = callback;
            return id;
        }

        _logger.Error($"Failed to register hotkey: {Marshal.GetLastWin32Error()}");
        return -1;
    }

    public void Unregister(int id)
    {
        if (_hotkeys.ContainsKey(id))
        {
            _logger.Error($"Failed to unregister hotkey with id: {id}");
            return;
        }

        bool success = User32.UnregisterHotKey(_source!.Handle, id);

        if (success)
        {
            _hotkeys.Remove(id);
            return;
        }

        _logger.Error($"Failed to unregister hotkey: {Marshal.GetLastWin32Error()}");
    }

    private nint HwndHook(nint _, int message, nint wParam, nint __, ref bool handled)
    {
        if (message != WM_HOTKEY)
            return IntPtr.Zero;

        int hotkeyId = (int)wParam;
        if (!_hotkeys.TryGetValue(hotkeyId, out Action? callback))
            return IntPtr.Zero;

        callback();
        handled = true;
        return IntPtr.Zero;
    }

    private static (int, int)? ParseHotkeyFrom(string keys)
    {
        if (keys == "None")
            return (0, 0);

        string[] combination = keys.Split("+");
        int modifier = 0x4000;

        int actualKey = 0;
        foreach (string key in combination)
            switch (key)
            {
                case "Alt":
                    modifier |= 0x0001;
                    break;
                case "Ctrl":
                    modifier |= 0x0002;
                    break;
                case "Shift":
                    modifier |= 0x0004;
                    break;
                default:
                    try
                    {
                        string parsedKey = CheckAndConvert(key);
                        actualKey = (int)Enum.Parse(typeof(KeyboardKeys), parsedKey);
                        break;
                    }
                    catch
                    {
                        return null;
                    }

            }

        return (modifier, actualKey);
    }

    private static string CheckAndConvert(string key)
    {
        string[] numbers = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
        return numbers.Contains(key) ? $"D{key}" : key;
    }

    public void Dispose()
    {
        _source?.RemoveHook(HwndHook);
        _source?.Dispose();
    }
}