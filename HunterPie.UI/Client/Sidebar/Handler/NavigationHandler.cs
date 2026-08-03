using HunterPie.UI.Architecture;
using HunterPie.UI.Client.Sidebar.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Media;

namespace HunterPie.UI.Client.Sidebar.Handler;

public abstract class NavigationHandler(
    string label,
    ImageSource icon,
    SideBarButtonState state = SideBarButtonState.Enabled
) : ViewModel, ILabeledNavigation
{
    public string Label { get; set => SetValue(ref field, value); } = label;

    public ImageSource Icon { get; set => SetValue(ref field, value); } = icon;

    public SideBarButtonState State { get; set => SetValue(ref field, value); } = state;

    public sealed class Group(
        string label,
        ImageSource icon,
        ObservableCollection<NavigationHandler> handlers
    ) : NavigationHandler(label, icon)
    {
        public IReadOnlyCollection<NavigationHandler> Handlers => handlers;
    }

    public abstract class Action(
        string label,
        ImageSource icon,
        SideBarButtonState state = SideBarButtonState.Enabled
    ) : NavigationHandler(label, icon, state)
    {
        public abstract Task ExecuteAsync();
    }

    public abstract class View(
        string label,
        ImageSource icon,
        Type viewType
    ) : NavigationHandler.Action(label, icon)
    {
        public Type ViewType { get; } = viewType;

        public bool IsActive { get; set => SetValue(ref field, value); }

        public abstract Task InitializeAsync();
    }

    public abstract class View<T>(
        string label,
        ImageSource icon
    ) : View(label, icon, typeof(T))
        where T : ViewModel;
}