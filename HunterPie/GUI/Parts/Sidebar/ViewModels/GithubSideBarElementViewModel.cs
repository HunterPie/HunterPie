using HunterPie.Core.Client.Localization;
using HunterPie.UI.Assets.Application;
using System.Diagnostics;
using System.Windows.Media;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels;
internal class GithubSideBarElementViewModel : ISideBarElement
{
    private const string GITHUB_URL = "https://github.com/Haato3o/HunterPie-v2";

    public ImageSource Icon => Resources.Icon("ICON_GITHUB");

    public string Text => Localization.QueryString("//Strings/Client/Tabs/Tab[@Id='GITHUB_STRING']");

    public bool IsActivable => false;

    public bool IsEnabled => true;

    public bool ShouldNotify => false;

    public void ExecuteOnClick() => Process.Start("explorer", GITHUB_URL);
}
