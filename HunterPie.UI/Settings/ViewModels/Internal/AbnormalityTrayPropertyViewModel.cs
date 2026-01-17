using HunterPie.Core.Client.Configuration.Overlay;
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
    IBodyNavigator bodyNavigator) : ConfigurationPropertyViewModel
{
    private readonly ConfigurationAdapter _configurationAdapter = configurationAdapter;
    private readonly IBodyNavigator _bodyNavigator = bodyNavigator;

    public ObservableCollection<AbnormalityWidgetConfig> Trays { get; } = trays;
    public GameProcessType Game { get; } = game;

    public void CreateNewTray()
    {
        Trays.Add(new AbnormalityWidgetConfig());
    }

    public void DeleteTray(AbnormalityWidgetConfig tray)
    {
        Trays.Remove(tray);
    }

    public void ConfigureTray(AbnormalityWidgetConfig tray)
    {
        ConfigurationCategoryGroup configuration = _configurationAdapter.Adapt(tray).First();
        ObservableCollection<AbnormalityCategoryViewModel> abnormalities = AbnormalityCategoryViewModelBuilder.Build(Game);

        var viewModel = new AbnormalityWidgetSettingsViewModel(
            configuration: configuration.Categories.First(),
            categories: abnormalities,
            selectedAbnormalities: tray.AllowedAbnormalities
        );

        _bodyNavigator.Navigate(viewModel);
    }
}