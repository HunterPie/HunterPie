using HunterPie.Core.Input;
using HunterPie.Domain.Interfaces;
using System;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace HunterPie.Internal.Initializers;

#nullable enable
internal class HotkeyInitializer : IInitializer, IDisposable
{
    private static HwndSource? _source;
    private static IntPtr _hWnd;

    public Task Init()
    {
        _hWnd = new WindowInteropHelper(App.Ui!)
            .EnsureHandle();

        _source = HwndSource.FromHwnd(_hWnd);

        _source?.AddHook(Hotkey.HwndHook);
        Hotkey.hWnd = _hWnd;

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _source?.RemoveHook(Hotkey.HwndHook);
        _source?.Dispose();
        _hWnd = IntPtr.Zero;
    }
}