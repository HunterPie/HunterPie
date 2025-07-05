﻿using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Environment.Activities;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player;
using HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;
using System.Windows.Threading;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.Controllers;

internal class MeowcenariesController : IContextHandler
{
    private readonly Dispatcher _mainDispatcher;
    private readonly MHRPlayer _player;
    private readonly MeowcenariesViewModel _viewModel;

    public MeowcenariesController(
        MHRContext context,
        MeowcenariesViewModel viewModel,
        Dispatcher mainDispatcher)
    {
        _mainDispatcher = mainDispatcher;
        _viewModel = viewModel;
        _player = context.Game.Player as MHRPlayer;
        UpdateData();
    }

    public void HookEvents()
    {
        _player.Meowmasters.OnDeployStateChange += OnDeployStateChange;
        _player.Meowmasters.OnStepChange += OnStepChange;
        _player.Meowmasters.OnBuddyCountChange += OnBuddyCountChange;
        _player.Meowmasters.OnExpectedOutcomeChange += OnExpectedOutcomeChange;
    }

    public void UnhookEvents()
    {
        _player.Meowmasters.OnDeployStateChange -= OnDeployStateChange;
        _player.Meowmasters.OnStepChange -= OnStepChange;
        _player.Meowmasters.OnBuddyCountChange -= OnBuddyCountChange;
        _player.Meowmasters.OnExpectedOutcomeChange -= OnExpectedOutcomeChange;
    }

    private void UpdateData()
    {
        _viewModel.Step = _player.Meowmasters.Step;
        _viewModel.MaxSteps = _player.Meowmasters.MaxSteps;
        _viewModel.SetOutcome(
            current: _player.Meowmasters.ExpectedOutcome,
            max: _player.Meowmasters.MaxOutcome
        );
        _viewModel.BuddyCount = _player.Meowmasters.BuddyCount;
        _viewModel.MaxBuddyCount = _player.Meowmasters.MaxBuddies;
        _viewModel.IsDeployed = _player.Meowmasters.IsDeployed;
    }

    private void OnExpectedOutcomeChange(object sender, MHRMeowmasters e)
    {
        _mainDispatcher.BeginInvoke(() =>
        {
            _viewModel.SetOutcome(
                current: e.ExpectedOutcome,
                max: e.MaxOutcome
            );
        });

    }

    private void OnBuddyCountChange(object sender, MHRMeowmasters e)
    {
        _viewModel.MaxBuddyCount = e.MaxBuddies;
        _viewModel.BuddyCount = e.BuddyCount;
        _mainDispatcher.BeginInvoke(() =>
        {
            _viewModel.SetOutcome(
                current: e.ExpectedOutcome,
                max: e.MaxOutcome
            );
        });
    }

    private void OnStepChange(object sender, MHRMeowmasters e)
    {
        _viewModel.Step = e.Step;
        _viewModel.MaxSteps = e.MaxSteps;
    }

    private void OnDeployStateChange(object sender, MHRMeowmasters e) => _viewModel.IsDeployed = e.IsDeployed;

}