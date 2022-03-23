using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.Widgets.Chat.ViewModels;
using System.Windows.Controls;

namespace HunterPie.UI.Overlay.Widgets.Chat.Views
{
    /// <summary>
    /// Interaction logic for ChatView.xaml
    /// </summary>
    public partial class ChatView : View<ChatViewModel>, IWidget<ChatWidgetConfig>, IWidgetWindow
    {
        public ChatView()
        {
            InitializeComponent();
        }

        public ChatWidgetConfig Settings => ClientConfig.Config.Overlay.ChatWidget;

        public string Title => "Chat Widget";

        public WidgetType Type => WidgetType.Window;

        IWidgetSettings IWidgetWindow.Settings => Settings;

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
}
