using HunterPie.Core.Game.Events;
using HunterPie.Integrations.Datasources.MonsterHunterWilds;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Player;
using HunterPie.UI.Overlay.Widgets.Activities.Wilds.ViewModels;

namespace HunterPie.UI.Overlay.Widgets.Activities.Wilds.Controllers;

internal class SupportShipController : IContextHandler
{
    private readonly MHWildsContext _context;
    private readonly SupportShipViewModel _viewModel;
    private MHWildsPlayer Player => (MHWildsPlayer)_context.Game.Player;

    public SupportShipController(
        MHWildsContext context,
        SupportShipViewModel viewModel)
    {
        _context = context;
        _viewModel = viewModel;

        UpdateData();
    }

    private void UpdateData()
    {
        _viewModel.Days = Player.SupportShip.Days;
        _viewModel.IsAvailable = Player.SupportShip.InTown;
    }

    public void HookEvents()
    {
        Player.SupportShip.DaysChanged += OnDaysChanged;
        Player.SupportShip.InTownChanged += OnInTownChanged;
    }

    public void UnhookEvents()
    {
        Player.SupportShip.DaysChanged -= OnDaysChanged;
        Player.SupportShip.InTownChanged -= OnInTownChanged;
    }

    private void OnInTownChanged(object sender, SimpleValueChangeEventArgs<bool> e)
    {
        _viewModel.IsAvailable = e.NewValue;
    }

    private void OnDaysChanged(object sender, SimpleValueChangeEventArgs<int> e)
    {
        _viewModel.Days = e.NewValue;
    }
}