using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity;
using HunterPie.UI.Overlay.Widgets.SpecializedTools.ViewModels;
using System;
using System.ComponentModel;

namespace HunterPie.UI.Overlay.Widgets.SpecializedTools.Controllers;

public class SpecializedToolController : IContextHandler
{
    private readonly IContext _context;
    private readonly ISpecializedTool _tool;
    private readonly SpecializedToolViewModelV2 _viewModel;
    private readonly SpecializedToolWidgetConfig _config;

    public SpecializedToolController(
        IContext context,
        ISpecializedTool tool,
        SpecializedToolViewModelV2 viewModel,
        SpecializedToolWidgetConfig config)
    {
        _context = context;
        _tool = tool;
        _viewModel = viewModel;
        _config = config;

        UpdateData();
    }

    public void HookEvents()
    {
        _context.Game.Player.OnVillageEnter += OnVillageUpdate;
        _context.Game.Player.OnVillageLeave += OnVillageUpdate;
        _tool.OnChange += OnToolChange;
        _tool.OnCooldownUpdate += OnCooldownUpdate;
        _tool.OnTimerUpdate += OnTimerUpdate;
        _config.IsShowOnlyInHuntingZoneEnabled.PropertyChanged += OnShowInHuntingZoneEnabledChanged;
    }

    public void UnhookEvents()
    {
        _context.Game.Player.OnVillageEnter -= OnVillageUpdate;
        _context.Game.Player.OnVillageLeave -= OnVillageUpdate;
        _tool.OnChange -= OnToolChange;
        _tool.OnCooldownUpdate -= OnCooldownUpdate;
        _tool.OnTimerUpdate -= OnTimerUpdate;
        _config.IsShowOnlyInHuntingZoneEnabled.PropertyChanged -= OnShowInHuntingZoneEnabledChanged;
    }

    private void OnTimerUpdate(object sender, ISpecializedTool e)
    {
        _viewModel.MaxTimer = e.MaxTimer;
        _viewModel.Timer = e.Timer;
        _viewModel.IsRecharging = e.Cooldown > 0 && _viewModel.Timer == 0;
    }

    private void OnCooldownUpdate(object sender, ISpecializedTool e)
    {
        _viewModel.MaxCooldown = e.MaxCooldown;
        _viewModel.Cooldown = e.MaxCooldown - e.Cooldown;
        _viewModel.IsRecharging = e.Cooldown > 0 && _viewModel.Timer == 0;
    }

    private void OnToolChange(object sender, ISpecializedTool e) => UpdateData();

    private void OnVillageUpdate(object sender, EventArgs e) => UpdateVisibility();

    private void OnShowInHuntingZoneEnabledChanged(object sender, PropertyChangedEventArgs e) => UpdateVisibility();

    private void UpdateData()
    {
        UpdateVisibility();
        _viewModel.Id = _tool.Id;
        _viewModel.MaxCooldown = _tool.MaxCooldown;
        _viewModel.Cooldown = _tool.Cooldown;
        _viewModel.MaxTimer = _tool.MaxTimer;
        _viewModel.Timer = _tool.Timer;
        _viewModel.IsRecharging = _viewModel.Cooldown > 0 && _viewModel.Timer == 0;

        if (_viewModel.Cooldown == 0 && _viewModel.Timer == 0)
            _viewModel.Timer = _viewModel.MaxTimer;
    }

    private void UpdateVisibility()
    {
        _viewModel.IsVisible = _context.Game.Player.InHuntingZone || !_config.IsShowOnlyInHuntingZoneEnabled;
    }
}