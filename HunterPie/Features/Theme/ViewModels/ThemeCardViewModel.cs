using HunterPie.UI.Architecture;
using System;
using System.Collections.ObjectModel;

namespace HunterPie.Features.Theme.ViewModels;

internal class ThemeCardViewModel : ViewModel
{
    private string _name = string.Empty;
    public string Name { get => _name; set => SetValue(ref _name, value); }

    private string _description = string.Empty;
    public string Description { get => _description; set => SetValue(ref _description, value); }

    private bool _isEnabled;
    public bool IsEnabled { get => _isEnabled; set => SetValue(ref _isEnabled, value); }

    private DateTime? _createdAt;
    public DateTime? CreatedAt { get => _createdAt; set => SetValue(ref _createdAt, value); }

    public ObservableCollection<string> Tags { get; } = new();
}