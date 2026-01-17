using HunterPie.Features.Theme.Controller;
using HunterPie.Features.Theme.ViewModels;
using HunterPie.UI.Architecture;
using HunterPie.UI.Navigation;
using System;
using System.Threading.Tasks;

namespace HunterPie.UI.SideBar.ViewModels;

internal class ThemeSideBarViewModel(
    IBodyNavigator bodyNavigator,
    ThemeHomeController themeHomeController) : ViewModel, ISideBarViewModel
{
    private readonly IBodyNavigator _bodyNavigator = bodyNavigator;
    private readonly ThemeHomeController _themeHomeController = themeHomeController;

    public Type Type => typeof(ThemeHomeViewModel);

    public string Label => "//Strings/Client/Tabs/Tab[@Id='THEMES_STRING']";

    public string Icon => "Icons.Palette";

    public bool IsAvailable => true;

    public bool IsSelected { get; set => SetValue(ref field, value); }

    public async Task ExecuteAsync()
    {
        _bodyNavigator.Navigate(
            viewModel: await _themeHomeController.GetViewModelAsync()
        );
    }
}