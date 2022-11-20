using HunterPie.Core.Address.Map;
using HunterPie.Core.Client;
using HunterPie.Core.Game;
using HunterPie.Core.Logger;
using HunterPie.Core.System;
using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Wirebug;
using System;

namespace HunterPie.Features.Overlay;

internal class WirebugWidgetInitializer : IWidgetInitializer
{
    private IContextHandler _handler;

    public void Load(Context context)
    {
        Core.Client.Configuration.OverlayConfig config = ClientConfigHelper.GetOverlayConfigFrom(ProcessManager.Game);

        if (!config.WirebugWidget.Initialize)
            return;

        if (context is MHRContext ctx)
        {
            PatchInGameHudAssembly(context);
            _handler = new WirebugWidgetContextHandler(ctx);
        }
    }

    public void Unload()
    {
        _handler?.UnhookEvents();
        _handler = null;
    }

    /*
        * MHR has a built-in option to hide the Wirebug visual elements,
        * however, turning that off also turns off the player's aim
        * This patch aims to patch that instruction that hides the aim
        * so it hides only the Wirebug elements.
        * 
        * Original instructions:
        * `and qword ptr[rax+50],-02`
        * 
        * Patched instructions:
        * `or qword ptr[rax+50], 01`
    */
    private void PatchInGameHudAssembly(Context context)
    {
        try
        {
            Core.Client.Configuration.OverlayConfig config = ClientConfigHelper.GetOverlayConfigFrom(ProcessManager.Game);

            if (!config.WirebugWidget.PatchInGameHud)
                return;

            long wirebugAimAddress = AddressMap.GetAbsolute("FUNC_WIREBUG_HIDE_AIM_ADDRESS");
            byte[] assembly =
            {
                // or qword ptr[rax+50], 01
                0x48, 0x83, 0x48, 0x50, 0x1
            };

            context.Process.Memory.InjectAsm(wirebugAimAddress, assembly);

            Log.Debug("Successfully patched Wirebug aim");
        }
        catch (Exception ex)
        {
            Log.Error("Failed to patch in-game Wirebug HUD: {0}", ex);
        }
    }
}
