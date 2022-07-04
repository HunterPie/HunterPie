using HunterPie.Core.Address.Map;
using HunterPie.Core.Client;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Rise;
using HunterPie.Core.Logger;
using HunterPie.Core.System;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Wirebug;

namespace HunterPie.Features.Overlay
{
    internal class WirebugWidgetInitializer : IWidgetInitializer
    {
        private IContextHandler _handler;

        public void Load(Context context)
        {
            var config = ClientConfigHelper.GetOverlayConfigFrom(ProcessManager.Game);

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
            var config = ClientConfigHelper.GetOverlayConfigFrom(ProcessManager.Game);

            if (!config.WirebugWidget.PatchInGameHud)
                return;

            long wirebugAimAddress = AddressMap.GetAbsolute("FUNC_WIREBUG_HIDE_AIM_ADDRESS");
            byte[] assembly =
            {
                // or qword ptr[rax+50], 01
                0x48, 0x83, 0x48, 0x50, 0x1
            };

            if (!context.Process.Memory.InjectAsm(wirebugAimAddress, assembly))
            {
                Log.Error("Failed to patch in-game Wirebug HUD");
                return;
            }

            Log.Debug("Successfully patched Wirebug aim");
        }
    }
}
