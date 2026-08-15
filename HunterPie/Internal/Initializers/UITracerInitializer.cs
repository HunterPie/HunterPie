using HunterPie.Core.Client.Configuration.Debug;
using HunterPie.Domain.Interfaces;
using HunterPie.Features.Observability.Tracing;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class UITracerInitializer(
    UITracer listener,
    DevelopmentConfig config
) : IInitializer
{
    public Task Init()
    {
        PresentationTraceSources.Refresh();
        _ = PresentationTraceSources.DataBindingSource.Listeners.Add(listener);
        PresentationTraceSources.DataBindingSource.Switch.Level = config.PresentationSourceLevel;

        return Task.CompletedTask;
    }
}