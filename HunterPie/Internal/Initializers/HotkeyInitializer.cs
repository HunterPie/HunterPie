using HunterPie.Core.Input;
using HunterPie.Domain.Interfaces;
using System;
using System.Windows.Interop;

namespace HunterPie.Internal.Initializers
{
    internal class HotkeyInitializer : IInitializer, IDisposable
    {
        private static HwndSource source;
        private static IntPtr hWnd;

        public void Init()
        {
            hWnd = new WindowInteropHelper(App.UI)
                .EnsureHandle();

            source = HwndSource.FromHwnd(hWnd);

            source.AddHook(Hotkey.HwndHook);
            Hotkey.hWnd = hWnd;
        }

        public void Dispose()
        {
            source.RemoveHook(Hotkey.HwndHook);
            source.Dispose();
            hWnd = IntPtr.Zero;
        }
    }
}
