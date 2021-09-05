using HunterPie.UI.Logger;
using System.Collections.ObjectModel;

namespace HunterPie.GUI.Parts.Console
{
    public class ConsoleViewModel
    {
        public ObservableCollection<LogString> Logs => HunterPieLogger.viewModel;
    }
}
