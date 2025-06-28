using HunterPie.Core.Game.Events;
using HunterPie.Integrations.Datasources.MonsterHunterWilds;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Player;
using HunterPie.UI.Overlay.Widgets.Activities.Wilds.ViewModels;

namespace HunterPie.UI.Overlay.Widgets.Activities.Wilds.Controllers;

internal class IngredientsCenterController : IContextHandler
{
    private readonly MHWildsContext _context;
    private readonly IngredientsCenterViewModel _viewModel;
    private MHWildsPlayer Player => (MHWildsPlayer)_context.Game.Player;

    public IngredientsCenterController(
        MHWildsContext context,
        IngredientsCenterViewModel viewModel)
    {
        _context = context;
        _viewModel = viewModel;

        UpdateData();
    }

    private void UpdateData()
    {
        _viewModel.Rations = Player.IngredientsCenter.Rations;
        _viewModel.IsFull = Player.IngredientsCenter.Rations == Player.IngredientsCenter.MaxRations;
    }

    public void HookEvents()
    {
        Player.IngredientsCenter.RationsChanged += OnRationsChanged;
    }

    public void UnhookEvents()
    {
        Player.IngredientsCenter.RationsChanged -= OnRationsChanged;
    }

    private void OnRationsChanged(object sender, CounterChangeEventArgs e)
    {
        _viewModel.Rations = e.Current;
        _viewModel.IsFull = e.Current == e.Max;
    }
}