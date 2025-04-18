﻿using HunterPie.UI.Architecture;
using HunterPie.UI.Navigation;
using System.Collections.ObjectModel;

namespace HunterPie.Features.Statistics.Details.ViewModels;
internal class QuestDetailsViewModel : ViewModel
{
    private int _selectedIndex;
    public int SelectedIndex { get => _selectedIndex; set => SetValue(ref _selectedIndex, value); }

    public ObservableCollection<MonsterDetailsViewModel> Monsters { get; } = new();

    [System.Obsolete]
    public void NavigateToPreviousPage() => Navigator.Body.Return();
}