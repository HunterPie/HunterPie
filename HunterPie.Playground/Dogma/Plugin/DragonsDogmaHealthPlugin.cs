using HunterPie.Core.Plugins.Entity;
using HunterPie.Playground.Dogma.Controller;
using HunterPie.Playground.Dogma.ViewModels;
using HunterPie.Playground.Dogma.Views;
using HunterPie.UI.Overlay.Service;

namespace HunterPie.Playground.Dogma.Plugin;

internal class DragonsDogmaHealthPlugin(
    IOverlay overlay,
    IWidgetProvider provider,
    DragonsDogmaMonsterWidgetController controller
) : IPlugin
{

    public Task InitializeAsync()
    {
        provider.Bind<DragonsDogmaMonstersViewModel, DragonsDogmaMonstersView>();

        controller.Initialize();

        overlay.Register(controller.ViewModel);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        controller.Dispose();
    }
}
