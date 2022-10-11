using HunterPie.Core.Architecture;
using System.Windows.Threading;

namespace HunterPie.UI.Architecture;

public class ViewModel : Bindable
{
    protected Dispatcher UIThread => Dispatcher.CurrentDispatcher;
}
