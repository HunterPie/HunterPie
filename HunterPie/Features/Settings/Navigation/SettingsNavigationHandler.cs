using HunterPie.Core.Client;
using HunterPie.Core.Client.Events;
using HunterPie.Core.Client.Localization;
using HunterPie.Features.Settings.Factory;
using HunterPie.Features.Settings.ViewModels;
using HunterPie.UI.Assets.Application;
using HunterPie.UI.Client.Sidebar.Handler;
using HunterPie.UI.Navigation;
using System.IO;
using System.Threading.Tasks;

namespace HunterPie.Features.Settings.Navigation;

internal class SettingsNavigationHandler(
    IBodyNavigator navigator,
    SettingsFactory settingsFactory,
    ILocalizationRepository localizationRepository
) : NavigationHandler<SettingsViewModel>(
    label: localizationRepository.FindStringBy("//Strings/Client/Tabs/Tab[@Id='SETTINGS_STRING']"),
    icon: Resources.Icon("ICON_SETTINGS")
)
{
    private SettingsViewModel? _viewModel;

    public override Task InitializeAsync()
    {
        ConfigManager.OnSync += OnConfigurationSync;
        return Task.CompletedTask;
    }

    public override async Task ExecuteAsync()
    {
        SettingsViewModel viewModel = await BuildViewModelAsync();

        navigator.Navigate(viewModel);
    }

    private async Task<SettingsViewModel> BuildViewModelAsync()
    {
        _viewModel = await settingsFactory.CreateFullAsync(currentGame: ClientConfig.Config.Client.LastConfiguredGame);
        return _viewModel;
    }

    private void OnConfigurationSync(object? sender, ConfigSaveEventArgs e)
    {
        if (Path.GetFileNameWithoutExtension(e.Path) != "config")
            return;

        if (_viewModel is not { })
            return;

        _viewModel.SynchronizedAt = e.SyncedAt;
    }
}