using HunterPie.Core.System;
using HunterPie.Domain.Common;
using HunterPie.UI.Assets.Application;
using HunterPie.UI.Home.ViewModels;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Home;

internal class HomeCallToActionsService
{
    public ObservableCollection<HomeCallToActionViewModel> GetAll() =>
        new ObservableCollection<HomeCallToActionViewModel>
        {
            new(
                icon: Resources.Icon("ICON_DISCORD"),
                title: "Discord",
                description: "Has any questions or technical issues? Get assistance from our team and community.",
                execute: () => BrowserService.OpenUrl(CommonLinks.DISCORD)
            ),
            new(
                icon: Resources.Icon("ICON_DOCUMENTATION"),
                title: "Documentation",
                description: "Learn more about HunterPie's features and how to use them.",
                execute: () => BrowserService.OpenUrl(CommonLinks.DOCUMENTATION)
            )
        };
}