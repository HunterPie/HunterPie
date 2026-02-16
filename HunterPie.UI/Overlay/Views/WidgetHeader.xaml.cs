using HunterPie.Core.Client;
using HunterPie.UI.Architecture.Media;
using HunterPie.UI.Overlay.ViewModels;
using System.Collections.Specialized;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace HunterPie.UI.Overlay.Views;

/// <summary>
/// Interaction logic for WidgetHeader.xaml
/// </summary>
public partial class WidgetHeader : UserControl
{
    public WidgetView Owner { get; private set; }

    public WidgetHeader()
    {
        InitializeComponent();
    }

    private void OnCloseButtonClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not WidgetContext vm)
            return;

        if (vm.ViewModel?.Settings?.Initialize is { } initialize)
            initialize.Value = false;
    }

    private void OnHideButtonClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not WidgetContext vm)
            return;

        vm.ViewModel.Settings.Enabled.Value = !vm.ViewModel.Settings.Enabled.Value;
    }

    private void OnLoaded(object sender, RoutedEventArgs e) => Owner = (WidgetView)Window.GetWindow(this);

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);
        Owner.DragMove();
    }

    private void OnSnapshotButtonClick(object sender, RoutedEventArgs e)
    {
        if (Owner.DataContext is not WidgetContext ctx)
            return;

        RenderTargetBitmap bitmap = Bitmap.From(
            element: Owner.PART_Content,
            scale: ctx.ViewModel.Settings.Scale.Current
        );

        string temporaryFile = ClientInfo.GetRandomTempFile() + ".png";

        using FileStream stream = File.OpenWrite(temporaryFile);
        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bitmap));
        encoder.Save(stream);

        var data = new DataObject();
        data.SetFileDropList(new StringCollection { temporaryFile });
        Clipboard.SetDataObject(data, true);
    }
}