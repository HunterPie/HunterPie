using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain.Dialog;
using HunterPie.Core.Domain.Enums;
using HunterPie.UI.Controls.Settings.Abnormality.Builders;
using HunterPie.UI.Controls.Settings.Abnormality.ViewModels;
using HunterPie.UI.Navigation;
using HunterPie.UI.Settings.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace HunterPie.UI.Settings.ViewModels.Internal;

internal class AbnormalityTrayPropertyViewModel(
    ObservableCollection<AbnormalityWidgetConfig> trays,
    GameProcessType game,
    ConfigurationAdapter configurationAdapter,
    IBodyNavigator bodyNavigator,
    AbnormalityCategoryViewModelBuilder categoryViewModelBuilder,
    ILocalizationRepository localizationRepository) : ConfigurationPropertyViewModel
{
    private readonly IScopedLocalizationRepository _dialogLocalizationRepository = localizationRepository.WithScope("//Strings/Client/Dialogs/Dialog");

    public ObservableCollection<AbnormalityWidgetConfig> Trays { get; } = trays;
    public GameProcessType Game { get; } = game;

    public void CreateNewTray()
    {
        Trays.Add(new AbnormalityWidgetConfig());
    }

    public void DeleteTray(AbnormalityWidgetConfig tray)
    {
        string title = _dialogLocalizationRepository.FindStringBy("CONFIRMATION_TITLE_STRING");
        string description = _dialogLocalizationRepository.FindStringBy("DELETE_CONFIRMATION_DESCRIPTION_STRING");

        NativeDialogResult result = DialogManager.Warn(
            title: title,
            description: description.Replace("{Item}", $"{(string)tray.Name}"),
            NativeDialogButtons.Accept | NativeDialogButtons.Cancel
        );

        if (result != NativeDialogResult.Accept)
            return;

        Trays.Remove(tray);
    }

    public void ConfigureTray(AbnormalityWidgetConfig tray)
    {
        ConfigurationCategoryGroup configuration = configurationAdapter.Adapt(tray).First();
        ObservableCollection<AbnormalityCategoryViewModel> abnormalities = categoryViewModelBuilder.Build(Game);

        var viewModel = new AbnormalityWidgetSettingsViewModel(
            configuration: configuration.Categories.First(),
            categories: abnormalities,
            selectedAbnormalities: tray.AllowedAbnormalities,
            bodyNavigator: bodyNavigator
        );

        bodyNavigator.Navigate(viewModel);
    }
}