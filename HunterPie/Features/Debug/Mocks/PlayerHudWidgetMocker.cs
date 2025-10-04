using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture.Test;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Player.ViewModels;

namespace HunterPie.Features.Debug.Mocks;

internal class PlayerHudWidgetMocker : IWidgetMocker
{
    public Observable<bool> Setting => ClientConfig.Config.Development.MockPlayerHudWidget;

    public WidgetView Mock(IOverlay overlay)
    {
        var config = new PlayerHudWidgetConfig();
        var viewModel = new PlayerHudViewModel(config)
        {
            Name = "Lyss",
            Level = 99,
            Health = 180.0,
            MaxHealth = 180.0,
            MaxExtendableHealth = 200.0,
            RecoverableHealth = 0,
            Stamina = 2000.0,
            MaxStamina = 2500.0,
            MaxPossibleStamina = 2500.0,
            Weapon = Weapon.SwitchAxe,
            InHuntingZone = true,
            SharpnessViewModel =
            {
                Sharpness = 50,
                MaxSharpness = 50
            }
        };


        MockBehavior.Run(() => viewModel.SharpnessViewModel.SharpnessLevel = (Sharpness)(((int)viewModel.SharpnessViewModel.SharpnessLevel + 1) % 7), 2);

        return overlay.Register(viewModel);
    }
}