using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HunterPie.UI.Architecture.Brushes;

public static class SnapshotBrush
{
    public static ImageBrush From(UIElement element)
    {
        double width = element.RenderSize.Width;
        double height = element.RenderSize.Height;
        DpiScale dpi = VisualTreeHelper.GetDpi(element);

        var bitmap = new RenderTargetBitmap(
            pixelWidth: (int)width,
            pixelHeight: (int)height,
            dpiX: dpi.DpiScaleX * dpi.PixelsPerInchX,
            dpiY: dpi.DpiScaleY * dpi.PixelsPerInchY,
            pixelFormat: PixelFormats.Pbgra32
        );
        bitmap.Render(element);

        return new ImageBrush(bitmap)
        {
            Stretch = Stretch.None,
            AlignmentX = AlignmentX.Left,
            AlignmentY = AlignmentY.Top,
        };
    }
}