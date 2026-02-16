using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Game;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Abnormality;
using HunterPie.UI.Overlay.Widgets.Abnormality.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Features.Overlay.Widgets;

internal class AbnormalitiesWidgetInitializer(IOverlay overlay) : IWidgetInitializer
{
    private readonly IOverlay _overlay = overlay;
    private readonly List<(IContextHandler, WidgetView)> _handlers = new();

    public GameProcessType SupportedGames =>
        GameProcessType.MonsterHunterRise
        | GameProcessType.MonsterHunterWorld
        | GameProcessType.MonsterHunterWilds;

    public Task LoadAsync(IContext context)
    {

        OverlayConfig config = ClientConfigHelper.GetOverlayConfigFrom(context.Process.Type);

        AbnormalityWidgetConfig[] configs = config.AbnormalityTray.Trays.Trays.ToArray();
        for (int i = 0; i < config.AbnormalityTray.Trays.Trays.Count; i++)
        {
            AbnormalityWidgetConfig widgetConfig = configs[i];

            if (!widgetConfig.Initialize)
                continue;

            var vm = new AbnormalityBarViewModel(widgetConfig);
            WidgetView view = _overlay.Register(vm);

            _handlers.Add(
                item: (
                    new AbnormalityWidgetContextHandler(
                        context: context,
                        config: widgetConfig,
                        viewModel: vm
                    ),
                    view
                )
            );
        }

        return Task.CompletedTask;
    }

    public void Unload()
    {
        foreach ((IContextHandler handler, WidgetView view) in _handlers)
        {
            handler.UnhookEvents();
            _overlay.Unregister(view);
        }

        _handlers.Clear();
    }
}