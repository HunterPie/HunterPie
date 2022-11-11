using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Core.System;
using HunterPie.UI.Overlay.Widgets.Player.ViewModels;
using HunterPie.UI.Overlay.Widgets.Player.Views;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.UI.Overlay.Widgets.Player;
public class PlayerHudWidgetContextHandler : IContextHandler
{
    private readonly HashSet<IAbnormality> _activeAbnormalities = new(15);
    private readonly PlayerHudView View;
    private readonly PlayerHudViewModel ViewModel;
    private readonly Context _context;
    private IPlayer Player => _context.Game.Player;

    public PlayerHudWidgetContextHandler(Context context)
    {
        PlayerHudWidgetConfig config = ClientConfigHelper.DeferOverlayConfig(ProcessManager.Game, (config) => config.PlayerHudWidget);

        View = new PlayerHudView(config);
        ViewModel = View.ViewModel;
        _context = context;
        _ = WidgetManager.Register<PlayerHudView, PlayerHudWidgetConfig>(View);

        HookEvents();
        UpdateData();
    }

    public void HookEvents()
    {
        Player.OnLogin += OnPlayerLogin;
        Player.OnLevelChange += OnPlayerLevelChange;
        Player.OnWeaponChange += OnPlayerWeaponChange;
        Player.OnStageUpdate += OnStageChange;
        Player.OnHealthChange += OnPlayerHealthChange;
        Player.OnStaminaChange += OnPlayerStaminaChange;
        Player.OnHeal += OnHeal;
        Player.OnAbnormalityStart += OnPlayerAbnormalityStart;
        Player.OnAbnormalityEnd += OnPlayerAbnormalityEnd;
    }

    private void OnPlayerAbnormalityEnd(object sender, IAbnormality e)
    {
        lock (_activeAbnormalities)
        {
            if (_activeAbnormalities.Contains(e))
                _activeAbnormalities.Remove(e);

            IAbnormality nextAbnormality = _activeAbnormalities.FirstOrDefault();

            if (nextAbnormality is null)
            {
                ViewModel.AbnormalityCategory = AbnormalityCategory.None;
                return;
            }

            ViewModel.AbnormalityCategory = _context.Game.AbnormalityCategorizationService.Categorize(nextAbnormality);
        }
    }

    private void OnPlayerAbnormalityStart(object sender, IAbnormality e)
    {
        AbnormalityCategory category = _context.Game.AbnormalityCategorizationService.Categorize(e);

        if (category == AbnormalityCategory.None)
            return;

        lock (_activeAbnormalities)
        {
            _activeAbnormalities.Add(e);
        }

        ViewModel.AbnormalityCategory = category;
    }
    private void OnHeal(object sender, HealthChangeEventArgs e) => ViewModel.Heal = e.Heal;
    private void OnStageChange(object sender, EventArgs e) => ViewModel.InHuntingZone = Player.InHuntingZone;
    private void OnPlayerLevelChange(object sender, LevelChangeEventArgs e) => ViewModel.Level = Player.MasterRank;
    private void OnPlayerWeaponChange(object sender, EventArgs e) => ViewModel.Weapon = Player.WeaponId;
    private void OnPlayerLogin(object sender, EventArgs e)
    {
        ViewModel.Name = Player.Name;
        ViewModel.Level = Player.MasterRank;
    }

    private void OnPlayerStaminaChange(object sender, StaminaChangeEventArgs e)
    {
        ViewModel.MaxStamina = e.MaxStamina;
        ViewModel.Stamina = e.Stamina;
        ViewModel.MaxPossibleStamina = e.MaxPossibleStamina;
        ViewModel.MaxRecoverableStamina = e.MaxRecoverableStamina;
    }

    private void OnPlayerHealthChange(object sender, HealthChangeEventArgs e)
    {
        ViewModel.MaxHealth = e.MaxHealth;
        ViewModel.MaxExtendableHealth = e.MaxPossibleHealth;
        ViewModel.Health = e.Health;
        ViewModel.RecoverableHealth = e.RecoverableHealth;
    }

    public void UnhookEvents()
    {
        Player.OnLogin -= OnPlayerLogin;
        Player.OnLevelChange -= OnPlayerLevelChange;
        Player.OnWeaponChange -= OnPlayerWeaponChange;
        Player.OnStageUpdate -= OnStageChange;
        Player.OnHealthChange -= OnPlayerHealthChange;
        Player.OnStaminaChange -= OnPlayerStaminaChange;
        Player.OnHeal -= OnHeal;

        _ = WidgetManager.Unregister<PlayerHudView, PlayerHudWidgetConfig>(View);
    }

    private void UpdateData()
    {
        ViewModel.MaxHealth = Player.MaxHealth;
        ViewModel.MaxExtendableHealth = Player.MaxPossibleHealth;
        ViewModel.Health = Player.Health;
        ViewModel.RecoverableHealth = Player.RecoverableHealth;
        ViewModel.MaxPossibleStamina = Player.MaxPossibleStamina;
        ViewModel.MaxRecoverableStamina = Player.MaxRecoverableStamina;
        ViewModel.MaxStamina = Player.MaxStamina;
        ViewModel.Stamina = Player.Stamina;
        ViewModel.Name = Player.Name;
        ViewModel.Level = Player.MasterRank;
    }
}
