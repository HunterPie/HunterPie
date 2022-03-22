using System.Windows;
using System.Windows.Media;

namespace HunterPie.UI.Overlay.Widgets.Chat.ViewModels
{
    internal class MockChatViewModel : ChatViewModel
    {
        public MockChatViewModel()
        {
            ChatElementViewModel[] chatElements =
            {
                new ChatElementViewModel()
                {
                    Text = "Hello, this is a string!",
                    Author = "Player 1",
                    Color = new SolidColorBrush(Color.FromArgb(0xFF, 0x93, 0x63, 0xEC))
                },
                new ChatElementViewModel()
                {
                    Text = "Hello, this is a string again!",
                    Author = "Player 1",
                    Color = new SolidColorBrush(Color.FromArgb(0xFF, 0x93, 0x63, 0xEC))
                },
                new ChatElementViewModel()
                {
                    Text = "Hello, this is a string again and again!",
                    Author = "Player 1",
                    Color = new SolidColorBrush(Color.FromArgb(0xFF, 0x93, 0x63, 0xEC))
                },
                new ChatElementViewModel()
                {
                    Text = "Hello, this is a string!",
                    Author = "Player 2",
                    Color = new SolidColorBrush(Color.FromArgb(0xFF, 0x42, 0x87, 0xf5))
                },
                new ChatElementViewModel()
                {
                    Text = "Hello, this is a string again!",
                    Author = "Player 1",
                    Color = new SolidColorBrush(Color.FromArgb(0xFF, 0x93, 0x63, 0xEC))
                },
                new ChatElementViewModel()
                {
                    Text = "Hello, this is a string again and again!",
                    Author = "Player 3",
                    Color = new SolidColorBrush(Color.FromArgb(0xFF, 0xF5, 0x42, 0x42))
                }
            };

            ChatCategoryViewModel general = new ChatCategoryViewModel()
            {
                Name = "General",
                Description = "General chat",
                Icon = (ImageSource)Application.Current.FindResource("ICON_DECORATION")
            };

            foreach (var chatElement in chatElements)
                general.Elements.Add(chatElement);

            Categories.Add(general);
        }
    }
}
