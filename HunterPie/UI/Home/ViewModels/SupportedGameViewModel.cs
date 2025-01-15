using HunterPie.Core.Domain.Process;
using HunterPie.UI.Architecture;
using System;

namespace HunterPie.UI.Home.ViewModels;

internal class SupportedGameViewModel : ViewModel
{
    private string _name = string.Empty;
    public string Name { get => _name; set => SetValue(ref _name, value); }

    private bool _isAvailable;
    public bool IsAvailable { get => _isAvailable; set => SetValue(ref _isAvailable, value); }

    private string? _banner;
    public string? Banner { get => _banner; set => SetValue(ref _banner, value); }

    private ProcessStatus _status = ProcessStatus.NotFound;
    public ProcessStatus Status { get => _status; set => SetValue(ref _status, value); }

    private Action? _execute;
    public Action? Execute { get => _execute; set => SetValue(ref _execute, value); }

    private Action? _onSettings;
    public Action? OnSettings { get => _onSettings; set => SetValue(ref _onSettings, value); }
}