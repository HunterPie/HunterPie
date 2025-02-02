using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Environment.Activities;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player;
using HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;
using System;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.Controllers;

internal class CohootNestController : IContextHandler
{
    private readonly MHRContext _context;
    private MHRPlayer Player => (MHRPlayer)_context.Game.Player;
    private readonly CohootNestViewModel _viewModel;

    public CohootNestController(
        MHRContext context,
        CohootNestViewModel viewModel)
    {
        _context = context;
        _viewModel = viewModel;
        UpdateData();
    }

    public void HookEvents()
    {
        Player.Cohoot.OnKamuraCountChange += OnKamuraCountChange;
        Player.Cohoot.OnElgadoCountChange += OnElgadoCountChange;
    }

    public void UnhookEvents()
    {
        Player.Cohoot.OnKamuraCountChange -= OnKamuraCountChange;
        Player.Cohoot.OnElgadoCountChange -= OnElgadoCountChange;

    }

    private void OnElgadoCountChange(object sender, MHRCohoot e)
    {
        _viewModel.ElgadoCount = e.ElgadoCount;
        _viewModel.ElgadoMaxCount = e.MaxCount;
        SetGeneralCount();
    }

    private void OnKamuraCountChange(object sender, MHRCohoot e)
    {
        _viewModel.KamuraCount = e.KamuraCount;
        _viewModel.KamuraMaxCount = e.MaxCount;
        SetGeneralCount();
    }

    public void UpdateData()
    {
        _viewModel.ElgadoCount = Player.Cohoot.ElgadoCount;
        _viewModel.ElgadoMaxCount = Player.Cohoot.MaxCount;
        _viewModel.KamuraCount = Player.Cohoot.KamuraCount;
        _viewModel.KamuraMaxCount = Player.Cohoot.MaxCount;
        SetGeneralCount();
    }

    private void SetGeneralCount()
    {
        _viewModel.Count = Math.Max(_viewModel.KamuraCount, _viewModel.ElgadoCount);
        _viewModel.MaxCount = Math.Max(_viewModel.KamuraMaxCount, _viewModel.ElgadoMaxCount);
    }
}