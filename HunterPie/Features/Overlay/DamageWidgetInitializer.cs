using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Game;
using HunterPie.Integrations.Datasources.MonsterHunterWilds;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Damage;
using HunterPie.UI.Overlay.Widgets.Damage.Controllers;
using HunterPie.UI.Overlay.Widgets.Damage.View;
using System.Threading.Tasks;

namespace HunterPie.Features.Overlay;

internal class DamageWidgetInitializer : IWidgetInitializer
{
    private IContextHandler? _handler;

    public GameProcessType SupportedGames =>
        GameProcessType.MonsterHunterRise
        | GameProcessType.MonsterHunterWorld
        | GameProcessType.MonsterHunterWilds;

    public Task LoadAsync(IContext context)
    {
        DamageMeterWidgetConfig config = ClientConfigHelper.DeferOverlayConfig(
            game: context.Process.Type,
            it => it.DamageMeterWidget
        );

        if (!config.Initialize)
            return Task.CompletedTask;


        _handler = context switch
        {
            MHWildsContext => new DamageMeterControllerV2(
                context: context,
                view: new MeterView(config),
                config: config
            ),
            _ => new DamageMeterWidgetContextHandler(context)
        };

        return Task.CompletedTask;
    }

    public void Unload()
    {
        _handler?.UnhookEvents();
        _handler = null;
    }
}