using HunterPie.Core.System;
using HunterPie.Domain.Common;
using HunterPie.UI.Architecture;
using HunterPie.UI.SideBar.ViewModels;
using System;

namespace HunterPie.Domain.Sidebar.Elements;

internal class GitHubSideBarViewModel : ViewModel, ISideBarViewModel
{
    public Type? Type => null;

    public string Label => "//Strings/Client/Tabs/Tab[@Id='GITHUB_STRING']";

    public string Icon => "ICON_GITHUB";

    public bool IsAvailable => true;

    public bool IsSelected { get; set; }

    public void Execute()
    {
        BrowserService.OpenUrl(CommonLinks.GITHUB);
    }
}