using HunterPie.UI.Architecture;
using System;
using System.Collections.ObjectModel;

namespace HunterPie.Features.Theme.ViewModels;

internal class ThemeCardViewModel : ViewModel
{
    public string Name { get; set => SetValue(ref field, value); } = string.Empty;
    public string Description { get; set => SetValue(ref field, value); } = string.Empty;
    public bool IsEnabled { get; set => SetValue(ref field, value); }
    public DateTime? CreatedAt { get; set => SetValue(ref field, value); }

    public ObservableCollection<string> Tags { get; } = new();
}