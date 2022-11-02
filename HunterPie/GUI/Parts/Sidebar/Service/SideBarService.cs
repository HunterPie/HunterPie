using HunterPie.GUI.Parts.Sidebar.ViewModels;

namespace HunterPie.GUI.Parts.Sidebar.Service;

#nullable enable
public static class SideBarService
{
    public delegate void SideBarEventHandler(ISideBarElement element);

    public static event SideBarEventHandler? NavigateToElement;
    public static ISideBarElement? CurrentlySelected { get; private set; }

    public static void Navigate(ISideBarElement? element)
    {
        if (element is null)
            return;

        CurrentlySelected = element;

        NavigateToElement?.Invoke(element);
        element.ExecuteOnClick();
    }
}
