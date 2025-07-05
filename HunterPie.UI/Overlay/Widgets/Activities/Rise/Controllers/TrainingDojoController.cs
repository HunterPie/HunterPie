using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Environment.Activities;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player;
using HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.Controllers;

internal class TrainingDojoController : IContextHandler
{
    private readonly MHRContext _context;
    private readonly TrainingDojoViewModel _viewModel;
    private MHRPlayer Player => (MHRPlayer)_context.Game.Player;


    public TrainingDojoController(
        MHRContext context,
        TrainingDojoViewModel viewModel)
    {
        _context = context;
        _viewModel = viewModel;
        ;

        UpdateData();
    }

    private void UpdateData()
    {
        _viewModel.Boosts = Player.TrainingDojo.Boosts;
        _viewModel.MaxBoosts = Player.TrainingDojo.MaxBoosts;
        _viewModel.Rounds = Player.TrainingDojo.Rounds;
        _viewModel.MaxRounds = Player.TrainingDojo.MaxRounds;
    }

    public void HookEvents()
    {
        Player.TrainingDojo.OnBoostsLeftChange += OnBoostsChange;
        Player.TrainingDojo.OnRoundsLeftChange += OnRoundsChange;
    }

    public void UnhookEvents()
    {
        Player.TrainingDojo.OnBoostsLeftChange -= OnBoostsChange;
        Player.TrainingDojo.OnRoundsLeftChange -= OnRoundsChange;
    }

    private void OnRoundsChange(object sender, MHRTrainingDojo e)
    {
        _viewModel.Rounds = e.Rounds;
        _viewModel.MaxRounds = e.MaxRounds;
    }

    private void OnBoostsChange(object sender, MHRTrainingDojo e)
    {
        _viewModel.Boosts = e.Boosts;
        _viewModel.MaxBoosts = e.MaxBoosts;
    }
}