using HunterPie.Core.Address.Map;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Game;
using HunterPie.Core.Observability.Logging;
using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Wirebug;
using HunterPie.UI.Overlay.Widgets.Wirebug.ViewModels;
using System;
using System.Threading.Tasks;

namespace HunterPie.Features.Overlay.Widgets;

internal class WirebugWidgetInitializer(
    IContext context,
    IOverlay overlay,
    OverlayConfig config
) : IWidgetInitializer
{
    private readonly ILogger _logger = LoggerFactory.Create();

    private readonly IOverlay _overlay = overlay;

    public GameProcessType SupportedGames => GameProcessType.MonsterHunterRise;

    private WirebugWidgetContextHandler? _handler;
    private WidgetView? _view;

    public async Task LoadAsync()
    {
        if (!config.WirebugWidget.Initialize)
            return;

        if (context is not MHRContext ctx)
            return;

        if (config.WirebugWidget.PatchInGameHud)
            await PatchInGameHudAssemblyAsync();

        var viewModel = new WirebugsViewModel(config.WirebugWidget);

        _handler = new WirebugWidgetContextHandler(
            context: ctx,
            viewModel: viewModel
        );
        _view = _overlay.Register(viewModel);
    }

    public void Dispose()
    {
        _overlay.Unregister(_view);
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
    private async Task PatchInGameHudAssemblyAsync()
    {
        try
        {
            nint wirebugAimAddress = AddressMap.GetAbsolute("FUNC_WIREBUG_HIDE_AIM_ADDRESS");
            byte[] assembly =
            {
                // or qword ptr[rax+50], 01
                0x48, 0x83, 0x48, 0x50, 0x1
            };

            await context.Process.Memory.InjectAsmAsync(wirebugAimAddress, assembly);

            _logger.Debug("Successfully patched Wirebug aim");
        }
        catch (Exception ex)
        {
            _logger.Error($"Failed to patch MHRise wirebug aim: {ex}");
        }
    }
}