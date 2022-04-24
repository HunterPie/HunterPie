using HunterPie.UI.Assets.Application;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using Localization = HunterPie.Core.Client.Localization.Localization;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels
{
    internal class DiscordSideBarElementViewModel : ISideBarElement
    {
        const string DiscordUrl = "https://discord.gg/5pdDq4Q";

        public ImageSource Icon => Resources.Icon("ICON_DISCORD");

        public string Text => Localization.Query("//Strings/Client/Tabs/Tab[@Id='DISCORD_STRING']").Attributes["String"].Value;

        public bool IsActivable => false;

        public bool IsEnabled => true;

        public bool ShouldNotify => false;

        public void ExecuteOnClick()
        {
            Process.Start("explorer", DiscordUrl);
        }
    }
}
