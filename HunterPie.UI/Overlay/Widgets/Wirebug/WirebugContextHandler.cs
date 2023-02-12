using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player;
using HunterPie.UI.Overlay.Widgets.Wirebug.ViewModel;

namespace HunterPie.UI.Overlay.Widgets.Wirebug;

internal class WirebugContextHandler : WirebugViewModel, IContextHandler
{
    public readonly MHRWirebug Context;

    public WirebugContextHandler(MHRWirebug context)
    {
        Context = context;

        UpdateData();
        HookEvents();
    }

    public void HookEvents()
    {
        Context.OnCooldownUpdate += OnCooldownUpdate;
        Context.OnTimerUpdate += OnTimerUpdate;
        Context.OnAvailable += OnAvailable;
        Context.OnWirebugStateChange += OnWirebugStateChange;
    }

    public void UnhookEvents()
    {
        Context.OnCooldownUpdate -= OnCooldownUpdate;
        Context.OnTimerUpdate -= OnTimerUpdate;
        Context.OnAvailable -= OnAvailable;
        Context.OnWirebugStateChange -= OnWirebugStateChange;
    }

    private void OnWirebugStateChange(object sender, MHRWirebug e) => WirebugState = e.WirebugState;

    private void OnTimerUpdate(object sender, MHRWirebug e)
    {
        MaxTimer = e.MaxTimer;
        Timer = e.Timer;
        IsTemporary = Timer > 0;
    }

    private void OnAvailable(object sender, MHRWirebug e) => IsAvailable = e.IsAvailable;

    private void OnCooldownUpdate(object sender, MHRWirebug e)
    {
        MaxCooldown = e.MaxCooldown == 0 ? 400 : e.MaxCooldown;
        Cooldown = e.Cooldown;
        OnCooldown = Cooldown > 0;
    }

    private void UpdateData()
    {
        WirebugState = Context.WirebugState;
        MaxCooldown = Context.MaxCooldown == 0 ? 400 : Context.MaxCooldown;
        Cooldown = Context.Cooldown;
        OnCooldown = Cooldown > 0;
        IsAvailable = Context.IsAvailable;

        MaxTimer = Context.MaxTimer;
        Timer = Context.Timer;
        IsTemporary = Timer > 0;
    }
}
