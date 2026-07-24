using HunterPie.Core.Observability.Logging;
using HunterPie.UI.Client.Sidebar.Handler;
using HunterPie.UI.Client.Sidebar.Service;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace HunterPie.UI.SideBar.Services;

internal class SideBarService(
    Dispatcher dispatcher
) : ISideBarRegistry, ISideBarCollection
{
    private readonly ILogger _logger = LoggerFactory.Create();

    private readonly Lock _lock = new();

    public ObservableCollection<INavigationHandler> Handlers { get; } = [];

    public async Task RegisterAsync(INavigationHandler handler) => await dispatcher.InvokeAsync(async () =>
    {
        await handler.InitializeAsync();

        lock (_lock)
        {
            Handlers.Add(handler);
        }

        _logger.Debug($"Registered new navigation handler: {handler.GetType().Name}");
    });
}