using HunterPie.UI.Architecture;
using System;
using System.Windows.Media;

namespace HunterPie.UI.Home.ViewModels;

internal class HomeCallToActionViewModel(ImageSource icon, string title, Action execute, string description) : ViewModel
{
    public ImageSource Icon { get; init; } = icon;
    public string Title { get; init; } = title;
    public string Description { get; init; } = description;
    public Action Execute { get; init; } = execute;
}