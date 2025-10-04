using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;

namespace HunterPie.Features.Debug.Mocks;

internal class MonsterWidgetMocker : IWidgetMocker
{
    public Observable<bool> Setting => ClientConfig.Config.Development.MockBossesWidget;

    public WidgetView Mock(IOverlay overlay)
    {
        var config = new MonsterWidgetConfig();
        var viewModel = new MonstersViewModel(config)
        {
            VisibleMonsters = 3,
            MonstersCount = 3
        };

        viewModel.Monsters.Add(new MockBossMonsterViewModel(config)
        {
            Name = "Monster",
            Em = "Rise_32",
            MaxHealth = 35000,
            Health = 35000,
            Stamina = 10000,
            MaxStamina = 10000,
            Crown = Crown.Gold,
            TargetType = Target.None,
            IsTarget = false,
            IsAlive = true,
            CaptureThreshold = 0.2,
            Variant = VariantType.Normal
        });
        viewModel.Monsters.Add(new MockBossMonsterViewModel(config)
        {
            Name = "Monster 2",
            Em = "Rise_32",
            MaxHealth = 35000,
            Health = 35000,
            Stamina = 10000,
            MaxStamina = 10000,
            Crown = Crown.Silver,
            TargetType = Target.None,
            IsTarget = false,
            IsAlive = true,
            CaptureThreshold = 0.25,
            Variant = VariantType.Tempered
        });
        viewModel.Monsters.Add(new MockBossMonsterViewModel(config)
        {
            Name = "Monster 3",
            Em = "Rise_32",
            MaxHealth = 35000,
            Health = 35000 * 0.53,
            Stamina = 8500,
            MaxStamina = 10000,
            Crown = Crown.Mini,
            TargetType = Target.None,
            IsTarget = false,
            IsAlive = true,
            CaptureThreshold = 0.5,
            IsCapturable = true,
            Variant = VariantType.Tempered | VariantType.Frenzy
        });

        viewModel.Monster = viewModel.Monsters[0];

        viewModel.Monsters.ForEach(it => it.FetchMonsterIcon());

        return overlay.Register(viewModel);
    }
}