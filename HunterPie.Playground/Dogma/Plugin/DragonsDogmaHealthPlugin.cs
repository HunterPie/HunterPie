using HunterPie.Core.Plugins.Entity;
using HunterPie.Playground.Dogma.Configuration;
using HunterPie.Playground.Dogma.Controller;
using HunterPie.Playground.Dogma.ViewModels;
using HunterPie.Playground.Dogma.Views;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;

namespace HunterPie.Playground.Dogma.Plugin;

internal class DragonsDogmaHealthPlugin(
    IOverlay overlay,
    IWidgetProvider provider,
    DragonsDogmaHealthPluginConfiguration config,
    DragonsDogmaMonsterWidgetController controller
) : IPlugin
{

    private WidgetView? _view;

    public Task InitializeAsync()
    {
        if (!config.MonsterWidget.Initialize)
            return Task.CompletedTask;

        provider.Bind<DragonsDogmaMonstersViewModel, DragonsDogmaMonstersView>();

        controller.Initialize();

        _view = overlay.Register(controller.ViewModel);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        if (_view is { })
            overlay.Unregister(_view);

        controller.Dispose();
    }
}