using HunterPie.Domain.Interfaces;
using HunterPie.Domain.Logger;
using System.Diagnostics;

namespace HunterPie.Internal.Intializers
{
    internal class UITracerInitializer : IInitializer
    {
        public void Init()
        {
            PresentationTraceSources.Refresh();
            PresentationTraceSources.DataBindingSource.Listeners.Add(new LogTracer());
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Warning;
        }
    }
}
