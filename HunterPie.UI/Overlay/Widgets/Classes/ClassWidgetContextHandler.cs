using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Client.Configuration.Overlay.Class;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Core.System;
using HunterPie.UI.Overlay.Widgets.Classes.Controllers;
using HunterPie.UI.Overlay.Widgets.Classes.ViewModels;
using HunterPie.UI.Overlay.Widgets.Classes.Views;

namespace HunterPie.UI.Overlay.Widgets.Classes;

#nullable enable
public class ClassWidgetContextHandler : IContextHandler
{
    private readonly ClassViewModel _viewModel;
    private readonly ClassView _view;
    private readonly IContext _context;
    private IClassController<IClassViewModel>? _weaponController;

    public ClassWidgetContextHandler(IContext context)
    {
        _view = new ClassView();
        _ = WidgetManager.Register<ClassView, ClassWidgetConfig>(_view);

        _viewModel = _view.ViewModel;
        _context = context;

        Update();
        HookEvents();
    }

    public void HookEvents()
    {
        _context.Game.Player.OnWeaponChange += OnWeaponChange;
    }

    public void UnhookEvents()
    {
        _context.Game.Player.OnWeaponChange -= OnWeaponChange;
        _ = WidgetManager.Unregister<ClassView, ClassWidgetConfig>(_view);
    }

    private void OnWeaponChange(object? sender, WeaponChangeEventArgs e)
    {
        UpdateConfig(e.NewWeapon);
        UpdateController(e.NewWeapon);
    }

    private void UpdateConfig(IWeapon weapon)
    {
        OverlayConfig overlayConfig = ClientConfigHelper.GetOverlayConfigFrom(ProcessManager.Game);
        ClassWidgetConfig? config = weapon.Id switch
        {
            Weapon.InsectGlaive => overlayConfig.InsectGlaiveWidget,
            _ => null
        };
        _viewModel.CurrentSettings = config;
    }

    private void UpdateController(IWeapon weapon)
    {
        _weaponController?.UnhookEvents();

        _weaponController = weapon switch
        {
            IInsectGlaive insectGlaive => new InsectGlaiveController(_context, insectGlaive),
            _ => null
        };
        _viewModel.Current = _weaponController?.ViewModel;
    }

    private void Update()
    {
        UpdateConfig(_context.Game.Player.Weapon);
        UpdateController(_context.Game.Player.Weapon);
    }
}
