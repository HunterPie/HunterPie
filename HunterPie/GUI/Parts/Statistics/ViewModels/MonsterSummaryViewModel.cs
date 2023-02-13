using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.UI.Architecture;

namespace HunterPie.GUI.Parts.Statistics.ViewModels;
internal class MonsterSummaryViewModel : ViewModel
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
}
