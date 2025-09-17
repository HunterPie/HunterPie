using System.Windows.Controls;

namespace HunterPie.UI.Overlay.Widgets.Chat.Views;

/// <summary>
/// Interaction logic for ChatView.xaml
/// </summary>
public partial class ChatView : Widget
{
    public ChatView()
    {
        InitializeComponent();
    }

    private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        if (sender is ScrollViewer scrollViewer)
        {
            double scrollableSize = scrollViewer.ViewportHeight;
            double scrollPosition = scrollViewer.VerticalOffset;
            double extentHeight = scrollViewer.ExtentHeight;

            if (scrollableSize + scrollPosition == extentHeight || extentHeight < scrollableSize)
                scrollViewer.ScrollToEnd();
        }
    }
}