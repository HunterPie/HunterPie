using HunterPie.Core.Client;
using HunterPie.Domain.Interfaces;
using HunterPie.Domain.Logger;
using System.Diagnostics;

namespace HunterPie.Internal.Initializers
{
    internal class UITracerInitializer : IInitializer
    {
        public void Init()
        {
            PresentationTraceSources.Refresh();
            PresentationTraceSources.DataBindingSource.Listeners.Add(new LogTracer());
            PresentationTraceSources.DataBindingSource.Switch.Level = ClientConfig.Config.Development.PresentationSourceLevel;
        }
    }
}
