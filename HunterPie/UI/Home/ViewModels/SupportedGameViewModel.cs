using HunterPie.Core.Domain.Process;
using HunterPie.UI.Architecture;
using System;

namespace HunterPie.UI.Home.ViewModels;

internal class SupportedGameViewModel : ViewModel
{
    public string Name { get; set => SetValue(ref field, value); } = string.Empty;
    public bool IsAvailable { get; set => SetValue(ref field, value); }
    public string? Banner { get; set => SetValue(ref field, value); }
    public ProcessStatus Status { get; set => SetValue(ref field, value); } = ProcessStatus.NotFound;
    public Delegate? Execute { get; set => SetValue(ref field, value); }
    public Delegate? OnSettings { get; set => SetValue(ref field, value); }
}