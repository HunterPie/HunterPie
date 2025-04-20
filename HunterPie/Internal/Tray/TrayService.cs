using System.Drawing;
using System.Windows.Forms;

namespace HunterPie.Internal.Tray;

public class TrayService
{
    private readonly ContextMenuStrip _contextMenuStrip = new();
    private readonly NotifyIcon _notifyIcon = new();
    private static TrayService? _instance;

    private TrayService(
        string tooltip,
        string text,
        Icon icon
    )
    {
        _notifyIcon.BalloonTipTitle = tooltip;
        _notifyIcon.Text = text;
        _notifyIcon.Icon = icon;
        _notifyIcon.Visible = true;
        _notifyIcon.ContextMenuStrip = _contextMenuStrip;
    }

    internal static void Initialize(
        string tooltip,
        string text,
        Icon icon
    ) => _instance = new TrayService(tooltip, text, icon);

    internal static void Dispose()
    {
        if (_instance is null)
            return;

        _instance._contextMenuStrip.Dispose();
        _instance._notifyIcon.Dispose();

        _instance = null;
    }

    public static void AddDoubleClickHandler(MouseEventHandler callback)
    {
        if (_instance is null)
            return;

        _instance._notifyIcon.MouseDoubleClick += callback;
    }

    public static ToolStripMenuItem? AddItem(string name)
    {
        return _instance?._contextMenuStrip.Items.Add(name) as ToolStripMenuItem;
    }

    public static ToolStripMenuItem? AddMenuItem(string name, ToolStripMenuItem menu) => menu.DropDownItems.Add(name) as ToolStripMenuItem;
}