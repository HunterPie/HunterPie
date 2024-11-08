using HunterPie.UI.Architecture;
using System;

namespace HunterPie.UI.Home.ViewModels;

internal class SupportedGameViewModel : ViewModel
{
    private string _name = string.Empty;
    public string Name { get => _name; set => SetValue(ref _name, value); }

    private bool _isAvailable;
    public bool IsAvailable { get => _isAvailable; set => SetValue(ref _isAvailable, value); }

    private string? _icon;
    public string? Icon { get => _icon; set => SetValue(ref _icon, value); }

    private bool _isHooking;
    public bool IsHooking { get => _isHooking; set => SetValue(ref _isHooking, value); }

    private Action? _execute;
    public Action? Execute { get => _execute; set => SetValue(ref _execute, value); }
}