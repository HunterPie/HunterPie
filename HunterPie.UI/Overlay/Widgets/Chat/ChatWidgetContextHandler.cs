using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Enums;
using HunterPie.UI.Assets.Application;
using HunterPie.UI.Overlay.Widgets.Chat.ViewModels;
using HunterPie.UI.Overlay.Widgets.Chat.Views;
using System.Windows.Media;

namespace HunterPie.UI.Overlay.Widgets.Chat
{
    public class ChatWidgetContextHandler : IContextHandler
    {
        private SolidColorBrush[] playerColors =
        {
            new SolidColorBrush(Color.FromArgb(0xFF, 0x54, 0x38, 0xDC)),
            new SolidColorBrush(Color.FromArgb(0xFF, 0x35, 0x7D, 0xED)),
            new SolidColorBrush(Color.FromArgb(0xFF, 0x56, 0xEE, 0xF4)),
            new SolidColorBrush(Color.FromArgb(0xFF, 0x32, 0xE8, 0x75))
        };
        
        private readonly ChatView View;
        private readonly ChatViewModel ViewModel;
        private readonly Context Context;
        public ChatCategoryViewModel General { get; } = new() 
        { 
            Name = "General", 
            Description = "General chat", 
            Icon = Resources.Icon<ImageSource>("ICON_STAR")
        };

        public ChatWidgetContextHandler(Context context)
        {
            Context = context;
            View = new ChatView();
            ViewModel = View.ViewModel;

            WidgetManager.Register<ChatView, ChatWidgetConfig>(View);

            UpdateData();
            HookEvents();
        }
        private void UpdateData()
        {
            ViewModel.Categories.Add(General);

            foreach (IChatMessage message in Context.Game.Chat.Messages)
            {
                View.Dispatcher.Invoke(() =>
                {
                    if (message.Type != AuthorType.Player1)
                        return;

                    General.Elements.Add(new ChatElementViewModel()
                    {
                        Author = message.Author,
                        Text = message.Message,
                        Color = playerColors[((int)message.Type - 2) % playerColors.Length]
                    });
                });
            }
        }

        public void HookEvents()
        {
            Context.Game.Chat.OnNewChatMessage += OnNewChatMessage;
        }

        public void UnhookEvents()
        {
            Context.Game.Chat.OnNewChatMessage -= OnNewChatMessage;

            WidgetManager.Unregister<ChatView, ChatWidgetConfig>(View);
        }
        private void OnNewChatMessage(object sender, IChatMessage e)
        {
            View.Dispatcher.Invoke(() =>
            {
                if (e.Type != AuthorType.Player1)
                    return;

                General.Elements.Add(new ChatElementViewModel()
                {
                    Author = e.Author,
                    Text = e.Message,
                    Color = playerColors[((int)e.Type - 2) % playerColors.Length]
                });
            });
        }
    }
}
