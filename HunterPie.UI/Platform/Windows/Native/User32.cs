using System;
using System.Runtime.InteropServices;

namespace HunterPie.UI.Platform.Windows.Native;

public static class User32
{
    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern uint SetWindowLong(IntPtr hwnd, int index, uint style);

    [DllImport("user32.dll")]
    public static extern uint GetWindowLong(IntPtr hwnd, int index);

    [DllImport("User32.dll", SetLastError = true)]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, nuint wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

    public const int GWL_EXSTYLE = -20;
    public const long HWND_BROADCAST = 0xFFFF;
    public const int WM_FONTCHANGE = 0x001D;

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

    public const uint DEFAULT_FLAGS = (uint)(SWP_WINDOWN_FLAGS.SWP_SHOWWINDOW
                                            | SWP_WINDOWN_FLAGS.SWP_NOMOVE
                                            | SWP_WINDOWN_FLAGS.SWP_NOSIZE
                                            | SWP_WINDOWN_FLAGS.SWP_NOACTIVATE);

    public const uint CLICK_THROUGH_FLAGS = (uint)(EX_WINDOW_STYLES.WS_EX_TRANSPARENT
                                                   | EX_WINDOW_STYLES.WS_EX_TOPMOST
                                                   | EX_WINDOW_STYLES.WS_EX_NOACTIVATE
                                                   | EX_WINDOW_STYLES.WS_EX_TOOLWINDOW);

    public const uint WINDOW_FLAGS = (uint)(EX_WINDOW_STYLES.WS_EX_TOPMOST
                                            | EX_WINDOW_STYLES.WS_EX_NOACTIVATE
                                            | EX_WINDOW_STYLES.WS_EX_TOOLWINDOW);
}