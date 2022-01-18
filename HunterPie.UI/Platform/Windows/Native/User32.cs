using System;
using System.Runtime.InteropServices;

namespace HunterPie.UI.Platform.Windows.Native
{
    public static class User32
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hwnd, int index, int style);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        public const int GWL_EXSTYLE = (-20);

        [Flags]
        public enum EX_WINDOW_STYLES : int
        {
            WS_EX_TOPMOST = 0x8,
            WS_EX_TRANSPARENT = 0x20,
            WS_EX_TOOLWINDOW = 0x80,
            WS_EX_NOACTIVATE = 0x08000000
        }

        [Flags]
        public enum SWP_WINDOWN_FLAGS : uint
        {
            SWP_SHOWWINDOW = 0x0040,
            SWP_NOMOVE = 0x0002,
            SWP_NOSIZE = 0x0001,
            SWP_NOACTIVATE = 0x0010
        }
    }
}
