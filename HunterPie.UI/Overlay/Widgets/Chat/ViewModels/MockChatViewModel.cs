using HunterPie.UI.Architecture.Test;
using HunterPie.UI.Assets.Application;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Media;

namespace HunterPie.UI.Overlay.Widgets.Chat.ViewModels
{
    internal class MockChatViewModel : ChatViewModel
    {
        public MockChatViewModel()
        {
            SolidColorBrush[] playerColors =
            {
                new SolidColorBrush(Color.FromArgb(0xFF, 0x54, 0x38, 0xDC)),
                new SolidColorBrush(Color.FromArgb(0xFF, 0x35, 0x7D, 0xED)),
                new SolidColorBrush(Color.FromArgb(0xFF, 0x56, 0xEE, 0xF4)),
                new SolidColorBrush(Color.FromArgb(0xFF, 0x32, 0xE8, 0x75))
            };

            string[] playerNames = { "Lyss", "Pia", "夜令", "MashiLo" };
            string[] possibleChats =
            {
                "poggies poggies wooo!",
                "uwu hi",
                "this is a test",
                "hi can someone help me",
                "deez nuts",
                "very cool",
                "subscribe to my onlyfans"
            };

            const int chatSize = 30;
            ChatElementViewModel[] chatElements = new ChatElementViewModel[chatSize];
            for (int i = 0; i < chatSize; i++)
            {
                Random rng = new Random();
                int playerIndex = rng.Next(0, playerNames.Length);
                int chatIndex = rng.Next(0, possibleChats.Length);
                chatElements[i] = new ChatElementViewModel()
                {
                    Author = playerNames[playerIndex],
                    Color = playerColors[playerIndex],
                    Text = possibleChats[chatIndex],
                };
            }

            ChatCategoryViewModel general = new ChatCategoryViewModel()
            {
                Name = "General",
                Description = "General chat",
                Icon = Resources.Icon<ImageSource>("ICON_STAR")
            };

            
            Categories.Add(general);

            IEnumerator enumerator = chatElements.GetEnumerator();
            MockBehavior.Run(() =>
            {
                bool success = enumerator.MoveNext();
                
                if (!success)
                    return;

                ChatElementViewModel vm = (ChatElementViewModel)enumerator.Current;

                Application.Current.Dispatcher.Invoke(() => general.Elements.Add(vm));
                                
            }, 1);
        }
    }
}
