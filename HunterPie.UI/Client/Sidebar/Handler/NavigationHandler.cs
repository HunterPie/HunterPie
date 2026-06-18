using HunterPie.UI.Architecture;
using HunterPie.UI.Client.Sidebar.Entity;
using System;
using System.Threading.Tasks;
using System.Windows.Media;

namespace HunterPie.UI.Client.Sidebar.Handler;

public abstract class NavigationHandler(
    string label,
    ImageSource icon
) : ViewModel, INavigationHandler
{
    public string Label { get; set => SetValue(ref field, value); } = label;

    public ImageSource Icon { get; set => SetValue(ref field, value); } = icon;

    public SideBarButtonState State { get; set => SetValue(ref field, value); }

    public bool IsActive { get; set => SetValue(ref field, value); }

    public abstract Task InitializeAsync();
    public abstract Task ExecuteAsync();

    public abstract class Action(
        string label,
        ImageSource icon
    ) : NavigationHandler(label, icon);

    public abstract class View(
        string label,
        ImageSource icon,
        Type viewType
    ) : NavigationHandler(label, icon)
    {
        public Type ViewType { get; } = viewType;
    }

    public abstract class View<T>(
        string label,
        ImageSource icon
    ) : View(label, icon, typeof(T))
        where T : ViewModel;
}