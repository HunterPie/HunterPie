using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Internal;
using HunterPie.UI.Architecture;
using HunterPie.UI.SideBar.ViewModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HunterPie.UI.Main.ViewModels;

internal class MainBodyViewModel(
    SideBarViewModel sideBarViewModel,
    ILocalRegistryAsync localRegistryAsync) : ViewModel
{
    private const string SUPPORTER_PROMPT_KEY = "supporter_prompt_closed";
    private readonly ILocalRegistryAsync _localRegistryAsync = localRegistryAsync;

    public SideBarViewModel SideBarViewModel { get; init; } = sideBarViewModel;
    public ViewModel? NavigationViewModel { get; set => SetValue(ref field, value); }

    public Observable<GameType> SelectedGame => ClientConfig.Config.Client.DefaultGameType;

    public ObservableCollection<GameType> Games { get; } = new() { GameType.Rise, GameType.World, GameType.Wilds };
    public bool ShouldDisplaySupporterPrompt { get; set => SetValue(ref field, value); }

    public void LaunchGame()
    {
        Steam.RunGameBy(SelectedGame.Value);
    }

    public async Task InitializeSupporterPromptAsync(bool isSupporter)
    {
        bool hasClosedPrompt = await _localRegistryAsync.ExistsAsync(SUPPORTER_PROMPT_KEY);

        ShouldDisplaySupporterPrompt = !hasClosedPrompt && !isSupporter;
    }

    public async Task CloseSupporterPromptAsync()
    {
        ShouldDisplaySupporterPrompt = false;
        await _localRegistryAsync.SetAsync(SUPPORTER_PROMPT_KEY, true);
    }
}