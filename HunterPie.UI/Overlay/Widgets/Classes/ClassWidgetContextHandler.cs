using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Client.Configuration.Overlay.Class;
using HunterPie.Core.Game;
using HunterPie.Core.System;
using HunterPie.UI.Overlay.Widgets.Classes.ViewModels;
using HunterPie.UI.Overlay.Widgets.Classes.Views;

namespace HunterPie.UI.Overlay.Widgets.Classes;
public class ClassWidgetContextHandler : IContextHandler
{
    private readonly ClassViewModel _viewModel;
    private readonly ClassView _view;
    private readonly IContext _context;

    public ClassWidgetContextHandler(IContext context)
    {


        _view = new ClassView();
        _ = WidgetManager.Register<ClassView, ClassWidgetConfig>(_view);

        _viewModel = _view.ViewModel;
        _context = context;

        HookEvents();
    }

    public void HookEvents()
    {
        OverlayConfig config = ClientConfigHelper.GetOverlayConfigFrom(ProcessManager.Game);
        _viewModel.CurrentSettings = config.InsectGlaiveWidget;
        _viewModel.Current = new InsectGlaiveViewModel();
    }

    public void UnhookEvents()
    {
        _ = WidgetManager.Unregister<ClassView, ClassWidgetConfig>(_view);
    }
}
