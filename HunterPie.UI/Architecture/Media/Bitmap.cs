using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HunterPie.UI.Architecture.Media;

public static class Bitmap
{
    public static RenderTargetBitmap From(
        UIElement element,
        double scale = 1.0)
    {
        double width = element.RenderSize.Width;
        double height = element.RenderSize.Height;
        DpiScale dpi = VisualTreeHelper.GetDpi(element);

        var bitmap = new RenderTargetBitmap(
            pixelWidth: (int)(width * scale),
            pixelHeight: (int)(height * scale),
            dpiX: dpi.DpiScaleX * dpi.PixelsPerInchX,
            dpiY: dpi.DpiScaleY * dpi.PixelsPerInchY,
            pixelFormat: PixelFormats.Pbgra32
        );
        bitmap.Render(element);

        if (bitmap.CanFreeze)
            bitmap.Freeze();

        return bitmap;
    }
}