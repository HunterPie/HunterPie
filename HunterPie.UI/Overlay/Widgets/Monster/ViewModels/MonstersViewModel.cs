using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Monster.ViewModels;

#nullable enable
public class MonstersViewModel(MonsterWidgetConfig settings) : WidgetViewModel(settings, "Monster Widget", WidgetType.ClickThrough)
{
    private const int MINIMUM_MONSTER_COUNT = 3;

    public ObservableCollection<BossMonsterViewModel> Monsters { get; } = new();
    public BossMonsterViewModel? Monster { get; set => SetValue(ref field, value); }
    public int VisibleMonsters { get; set => SetValue(ref field, value); }
    public int MonstersCount { get; set => SetValue(ref field, Math.Max(MINIMUM_MONSTER_COUNT, value)); }

    public MonsterWidgetConfig Config { get; } = settings;
}