using HunterPie.UI.Architecture;
using HunterPie.UI.Logging.Entity;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Logging.ViewModels;

internal class ConsoleViewModel(ObservableCollection<LogString> logs) : ViewModel
{
    public ObservableCollection<LogString> Logs { get; } = logs;
}