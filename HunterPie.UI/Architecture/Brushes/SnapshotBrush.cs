using HunterPie.UI.Architecture.Media;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HunterPie.UI.Architecture.Brushes;

public static class SnapshotBrush
{
    public static ImageBrush From(UIElement element)
    {
        RenderTargetBitmap bitmap = Bitmap.From(element);

        return new ImageBrush(bitmap)
        {
            Stretch = Stretch.None,
            AlignmentX = AlignmentX.Left,
            AlignmentY = AlignmentY.Top,
        };
    }
}