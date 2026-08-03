using HunterPie.Core.Observability.Logging;
using HunterPie.UI.Client.Sidebar.Handler;
using HunterPie.UI.Client.Sidebar.Service;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace HunterPie.UI.SideBar.Services;

internal class SideBarService(
    Dispatcher dispatcher
) : ISideBarRegistry, IFixedSideBarRegistry, ISideBarCollection
{
    private readonly ILogger _logger = LoggerFactory.Create();

    private readonly Lock _lock = new();

    private readonly ObservableCollection<NavigationHandler> _handlers = [];

    private readonly ObservableCollection<NavigationHandler> _fixedHandlers = [];

    public IReadOnlyCollection<NavigationHandler> Handlers => _handlers;

    public IReadOnlyCollection<NavigationHandler> FixedHandlers => _fixedHandlers;

    public async Task RegisterAsync(NavigationHandler handler) => await dispatcher.InvokeAsync(async () =>
    {
        if (handler is NavigationHandler.View viewHandler)
            await viewHandler.InitializeAsync();

        lock (_lock)
        {
            _handlers.Add(handler);
        }

        _logger.Debug($"Registered new navigation handler: {handler.GetType().Name}");
    });

    public async Task RegisterFixedAsync(NavigationHandler handler) => await dispatcher.InvokeAsync(async () =>
    {
        if (handler is NavigationHandler.View viewHandler)
            await viewHandler.InitializeAsync();

        lock (_lock)
        {
            _fixedHandlers.Add(handler);
        }

        _logger.Debug($"Registered new fixed navigation handler: {handler.GetType().Name}");
    });
}