using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.Widgets.Chat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
    }
}
