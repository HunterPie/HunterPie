using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Remote;
using HunterPie.Integrations.Poogie.Statistics.Models;
using HunterPie.UI.Architecture;
using System.IO;
using System.Threading.Tasks;

namespace HunterPie.GUI.Parts.Statistics.ViewModels;
public class MonsterSummaryViewModel : ViewModel
{
    private GameType _gameType;
    public GameType GameType
    {
        get => _gameType;
        set => SetValue(ref _gameType, value);
    }

    private int _id;
    public int Id
    {
        get => _id;
        set => SetValue(ref _id, value);
    }

    private string _icon;
    public string Icon
    {
        get => _icon;
        set => SetValue(ref _icon, value);
    }

    public MonsterSummaryViewModel() { }

    internal MonsterSummaryViewModel(GameType game, PoogieMonsterSummaryModel model)
    {
        GameType = game;
        Id = model.Id;

        FetchMonsterIcon();
    }

    private async Task FetchMonsterIcon()
    {
        string imageName = GetEm();
        string imagePath = ClientInfo.GetPathFor($"Assets/Monsters/Icons/{imageName}.png");

        // If file doesn't exist locally, we can check for the CDN
        if (!File.Exists(imagePath))
            imagePath = await CDN.GetMonsterIconUrl(imageName);

        Icon = imagePath;
    }

    private string GetEm() => $"{GameType}_{Id:00}";
}
