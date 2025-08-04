using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity;
using HunterPie.UI.Overlay.Widgets.SpecializedTools.ViewModels;
using HunterPie.UI.Overlay.Widgets.SpecializedTools.Views;
using System;

namespace HunterPie.UI.Overlay.Widgets.SpecializedTools.Controllers;

public class SpecializedToolController : IContextHandler
{
    private readonly IContext _context;
    private readonly ISpecializedTool _tool;
    private readonly SpecializedToolViewV2 _view;
    private readonly SpecializedToolViewModelV2 _viewModel;
    private readonly SpecializedToolWidgetConfig _config;

    public SpecializedToolController(
        IContext context,
        ISpecializedTool tool,
        SpecializedToolViewV2 view,
        SpecializedToolWidgetConfig config)
    {
        _context = context;
        _tool = tool;
        _view = view;
        _viewModel = view.ViewModel;
        _config = config;

        UpdateData();

        WidgetManager.Register<SpecializedToolViewV2, SpecializedToolWidgetConfig>(_view);
    }

    public void HookEvents()
    {
        _context.Game.Player.OnVillageEnter += OnVillageUpdate;
        _context.Game.Player.OnVillageLeave += OnVillageUpdate;
        _tool.OnChange += OnToolChange;
        _tool.OnCooldownUpdate += OnCooldownUpdate;
        _tool.OnTimerUpdate += OnTimerUpdate;
    }

    public void UnhookEvents()
    {
        _context.Game.Player.OnVillageEnter -= OnVillageUpdate;
        _context.Game.Player.OnVillageLeave -= OnVillageUpdate;
        _tool.OnChange -= OnToolChange;
        _tool.OnCooldownUpdate -= OnCooldownUpdate;
        _tool.OnTimerUpdate -= OnTimerUpdate;

        WidgetManager.Unregister<SpecializedToolViewV2, SpecializedToolWidgetConfig>(_view);
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


    private void OnVillageUpdate(object sender, EventArgs e)
    {
        _viewModel.IsVisible = _context.Game.Player.InHuntingZone;
    }

    private void UpdateData()
    {
        _viewModel.IsVisible = _context.Game.Player.InHuntingZone;
        _viewModel.Id = _tool.Id;
        _viewModel.MaxCooldown = _tool.MaxCooldown;
        _viewModel.Cooldown = _tool.Cooldown;
        _viewModel.MaxTimer = _tool.MaxTimer;
        _viewModel.Timer = _tool.Timer;
        _viewModel.IsRecharging = _viewModel.Cooldown > 0 && _viewModel.Timer == 0;

        if (_viewModel.Cooldown == 0 && _viewModel.Timer == 0)
            _viewModel.Timer = _viewModel.MaxTimer;
    }
}