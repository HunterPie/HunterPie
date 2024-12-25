using HunterPie.UI.Architecture;
using HunterPie.UI.Logger;
using HunterPie.UI.Logging.ViewModels;
using HunterPie.UI.Navigation;
using System;

namespace HunterPie.UI.SideBar.ViewModels;

internal class ConsoleSideBarViewModel : ViewModel, ISideBarViewModel
{
    public Type Type => typeof(ConsoleViewModel);

    public string Label => "//Strings/Client/Tabs/Tab[@Id='CONSOLE_STRING']";

    public string Icon => "ICON_CONSOLE";

    public bool IsAvailable => true;

    private bool _isSelected;
    public bool IsSelected { get => _isSelected; set => SetValue(ref _isSelected, value); }

    public void Execute()
    {
        Navigator.Body.Navigate(new ConsoleViewModel(HunterPieLogger.ViewModel));
    }
}