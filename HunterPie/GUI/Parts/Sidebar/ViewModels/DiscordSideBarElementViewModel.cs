using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels
{
    internal class DiscordSideBarElementViewModel : ISideBarElement
    {
        const string DiscordUrl = "https://discord.gg/5pdDq4Q";

        public ImageSource Icon => Application.Current.FindResource("ICON_DISCORD") as ImageSource;

        public string Text => "Discord";

        public bool IsActivable => false;

        public void ExecuteOnClick()
        {
            Process.Start("explorer", DiscordUrl);
        }
    }
}
