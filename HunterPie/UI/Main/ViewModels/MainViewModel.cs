using HunterPie.Core.Client;
using HunterPie.Features.Account.Config;
using HunterPie.UI.Architecture;
using HunterPie.UI.Header.ViewModels;
using System.Threading.Tasks;

namespace HunterPie.UI.Main.ViewModels;

internal class MainViewModel : ViewModel
{
    private readonly RemoteAccountConfigService _remoteConfigService = new();

    public HeaderViewModel HeaderViewModel { get; init; }

    private ViewModel? _contentViewModel;
    public ViewModel? ContentViewModel { get => _contentViewModel; set => SetValue(ref _contentViewModel, value); }

    public MainViewModel(
        HeaderViewModel headerViewModel
    )
    {
        HeaderViewModel = headerViewModel;
    }

    public async Task GracefulShutdown()
    {
        ConfigManager.SaveAll();
        await _remoteConfigService.UploadClientConfig();
    }
}