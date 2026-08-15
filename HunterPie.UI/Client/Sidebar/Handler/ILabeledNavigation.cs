using System.Windows.Media;

namespace HunterPie.UI.Client.Sidebar.Handler;

public interface ILabeledNavigation
{
    public string Label { get; }

    public ImageSource Icon { get; }
}