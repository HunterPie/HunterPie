using HunterPie.Core.Input;
using HunterPie.Domain.Interfaces;
using System;
using System.Windows;
using System.Windows.Interop;

namespace HunterPie.Internal.Intializers
{
    internal class HotkeyInitializer : IInitializer, IDisposable
    {
        private static HwndSource source;
        private static IntPtr hWnd;

        public void Init()
        {
            hWnd = new WindowInteropHelper(Application.Current.MainWindow).EnsureHandle();
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
