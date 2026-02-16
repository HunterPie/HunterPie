using HunterPie.UI.Architecture;
using HunterPie.UI.Logging.ViewModels;
using HunterPie.UI.Navigation;
using System;
using System.Threading.Tasks;

namespace HunterPie.UI.SideBar.ViewModels;

internal class ConsoleSideBarViewModel(
    IBodyNavigator bodyNavigator,
    ConsoleViewModel consoleViewModel) : ViewModel, ISideBarViewModel
{
    private readonly IBodyNavigator _bodyNavigator = bodyNavigator;
    private readonly ConsoleViewModel _consoleViewModel = consoleViewModel;

    public Type Type => typeof(ConsoleViewModel);

    public string Label => "//Strings/Client/Tabs/Tab[@Id='CONSOLE_STRING']";

    public string Icon => "ICON_CONSOLE";

    public bool IsAvailable => true;

    public bool IsSelected { get; set => SetValue(ref field, value); }

    public Task ExecuteAsync()
    {
        _bodyNavigator.Navigate(_consoleViewModel);

        return Task.CompletedTask;
    }
}