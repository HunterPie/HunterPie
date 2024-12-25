using HunterPie.Core.System;
using HunterPie.Domain.Common;
using HunterPie.UI.Architecture;
using System;

namespace HunterPie.UI.SideBar.ViewModels;

internal class DiscordSideBarViewModel : ViewModel, ISideBarViewModel
{
    public Type? Type => null;

    public string Label => "//Strings/Client/Tabs/Tab[@Id='DISCORD_STRING']";

    public string Icon => "ICON_DISCORD";

    public bool IsAvailable => true;

    public bool IsSelected { get; set; }

    public void Execute()
    {
        BrowserService.OpenUrl(CommonLinks.DISCORD);
    }
}