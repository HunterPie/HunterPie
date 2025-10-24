using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay.Class;
using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture.Test;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Classes.ViewModels;

namespace HunterPie.Features.Debug.Mocks;

internal class InsectGlaiveWidgetMocker : IWidgetMocker
{
    public Observable<bool> Setting => ClientConfig.Config.Development.MockInsectGlaiveWidget;

    public WidgetView Mock(IOverlay overlay)
    {
        var mockSettings = new ClassWidgetConfig();
        var viewModel = new ClassViewModel(mockSettings)
        {
            InHuntingZone = true,
            Current = MockViewModel()
        };

        return overlay.Register(viewModel);
    }

    private static InsectGlaiveViewModel MockViewModel()
    {
        var vm = new InsectGlaiveViewModel()
        {
            AttackTimer = 90,
            ChargeTimer = 250,
            ChargeType = KinsectChargeType.None,
            DefenseTimer = 90,
            MaxStamina = 150,
            Stamina = 150,
            MovementSpeedTimer = 90,
            PrimaryQueuedBuff = KinsectBuff.Attack,
            SecondaryQueuedBuff = KinsectBuff.Defense
        };

        MockBehavior.Run((() =>
        {
            vm.Stamina = vm.Stamina <= 0 ? 90 : vm.Stamina - 1;
            vm.AttackTimer = vm.AttackTimer <= 0 ? 90 : vm.AttackTimer - 1;
            vm.MovementSpeedTimer = vm.MovementSpeedTimer <= 0 ? 90 : vm.MovementSpeedTimer - 1;
            vm.DefenseTimer = vm.DefenseTimer <= 0 ? 90 : vm.DefenseTimer - 1;
            vm.ChargeTimer = vm.ChargeTimer <= 0 ? 90 : vm.ChargeTimer - 1;

        }));

        return vm;
    }
}