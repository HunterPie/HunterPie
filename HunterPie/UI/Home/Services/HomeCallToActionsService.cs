using HunterPie.Core.Client.Localization;
using HunterPie.Core.Extensions;
using HunterPie.Core.System;
using HunterPie.Domain.Common;
using HunterPie.UI.Assets.Application;
using HunterPie.UI.Home.ViewModels;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Home.Services;

internal class HomeCallToActionsService(
    ILocalizationRepository localizationRepository
)
{
    public ObservableCollection<HomeCallToActionViewModel> GetAll() =>
        [
            localizationRepository.FindBy("//Strings/Client/Home/CallToAction[@Id='DISCORD_CALL_TO_ACTION']")
                .Let((resolved) =>
                {
                    return new HomeCallToActionViewModel(
                        icon: Resources.Icon("ICON_DISCORD"),
                        title: resolved.String,
                        description: resolved.Description,
                        execute: () => BrowserService.OpenUrl(CommonLinks.DISCORD)
                    );
                })!
            ,
            localizationRepository.FindBy("//Strings/Client/Home/CallToAction[@Id='DOCUMENTATION_CALL_TO_ACTION']")
                .Let((resolved) =>
                {
                    return new HomeCallToActionViewModel(
                        icon: Resources.Icon("ICON_DOCUMENTATION"),
                        title: resolved.String,
                        description: resolved.Description,
                        execute: () => BrowserService.OpenUrl(CommonLinks.DOCUMENTATION)
                    );
                })!
        ];
}