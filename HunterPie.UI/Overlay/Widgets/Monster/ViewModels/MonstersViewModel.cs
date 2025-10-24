using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Monster.ViewModels;

#nullable enable
public class MonstersViewModel : WidgetViewModel
{
    private const int MINIMUM_MONSTER_COUNT = 3;

    public ObservableCollection<BossMonsterViewModel> Monsters { get; } = new();

    private BossMonsterViewModel? _monster;
    public BossMonsterViewModel? Monster { get => _monster; set => SetValue(ref _monster, value); }

    private int _visibleMonsters;
    public int VisibleMonsters { get => _visibleMonsters; set => SetValue(ref _visibleMonsters, value); }

    private int _monstersCount;
    public int MonstersCount { get => _monstersCount; set => SetValue(ref _monstersCount, Math.Max(MINIMUM_MONSTER_COUNT, value)); }

    public MonsterWidgetConfig Config { get; }

    public MonstersViewModel(MonsterWidgetConfig settings) : base(settings, "Monster Widget", WidgetType.ClickThrough)
    {
        Config = settings;
    }
}