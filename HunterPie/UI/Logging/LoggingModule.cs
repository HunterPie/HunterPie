using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.UI.Logging.Entity;
using HunterPie.UI.Logging.Services;
using HunterPie.UI.Logging.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace HunterPie.UI.Logging;

internal class LoggingModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        var stream = new ObservableCollection<LogString>();

        registry
            .WithSingle(_ => new ConsoleViewModel(stream))
            .WithSingle(r => new HunterPieLogWriter(
                dispatcher: r.Get<Dispatcher>(),
                logs: stream
            ));
    }
}