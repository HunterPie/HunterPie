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
    public virtual Type ViewType { get; protected set; } = typeof(void);

    public string Label { get; set => SetValue(ref field, value); } = label;

    public ImageSource Icon { get; set => SetValue(ref field, value); } = icon;

    public SideBarButtonState State { get; set => SetValue(ref field, value); }

    public bool IsActive { get; set => SetValue(ref field, value); }

    public abstract Task InitializeAsync();
    public abstract Task ExecuteAsync();
}

public abstract class NavigationHandler<T>(
    string label,
    ImageSource icon
) : NavigationHandler(label, icon)
    where T : ViewModel
{
    public override Type ViewType { get; protected set; } = typeof(T);
}