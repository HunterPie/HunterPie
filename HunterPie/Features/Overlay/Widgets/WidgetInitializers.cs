using HunterPie.Core.Game;
using HunterPie.Core.Observability.Logging;
using HunterPie.UI.Architecture.Overlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Features.Overlay.Widgets;

internal class WidgetInitializers(IWidgetInitializer[] initializers)
{
    private readonly ILogger _logger = LoggerFactory.Create();
    private readonly IWidgetInitializer[] _initializers = initializers;

    public async Task InitializeAsync(Context context)
    {
        IEnumerable<Task> tasks = _initializers
            .Where(it => it.SupportedGames.HasFlag(context.Process.Type))
            .Select(it => it.LoadAsync(context));

        try
        {
            await Task.WhenAll(tasks);
        }
        catch (Exception err)
        {
            _logger.Error(err.ToString());
        }
    }

    public void Unload()
    {
        foreach (IWidgetInitializer initializer in _initializers)
            initializer.Unload();
    }
}