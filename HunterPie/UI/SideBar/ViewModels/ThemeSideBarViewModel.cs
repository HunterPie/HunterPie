using HunterPie.Features.Theme.Controller;
using HunterPie.Features.Theme.ViewModels;
using HunterPie.UI.Architecture;
using HunterPie.UI.Navigation;
using System;
using System.Threading.Tasks;

namespace HunterPie.UI.SideBar.ViewModels;

internal class ThemeSideBarViewModel : ViewModel, ISideBarViewModel
{
    private readonly IBodyNavigator _bodyNavigator;
    private readonly ThemeHomeController _themeHomeController;

    public Type Type => typeof(ThemeHomeViewModel);

    public string Label => "//Strings/Client/Tabs/Tab[@Id='THEME_STRING']";

    public string Icon => "Icons.Palette";

    public bool IsAvailable => true;

    private bool _isSelected;
    public bool IsSelected { get => _isSelected; set => SetValue(ref _isSelected, value); }

    public ThemeSideBarViewModel(
        IBodyNavigator bodyNavigator,
        ThemeHomeController themeHomeController)
    {
        _bodyNavigator = bodyNavigator;
        _themeHomeController = themeHomeController;
    }

    public async Task ExecuteAsync()
    {
        _bodyNavigator.Navigate(
            viewModel: await _themeHomeController.GetViewModelAsync()
        );
    }
}