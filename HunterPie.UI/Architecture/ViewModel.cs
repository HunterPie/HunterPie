using HunterPie.Core.Architecture;
using System.Windows;
using System.Windows.Threading;

namespace HunterPie.UI.Architecture;

public class ViewModel : Bindable
{
    protected Dispatcher UIThread => Application.Current.Dispatcher;
}
