using HunterPie.Core.Extensions;
using HunterPie.Playground.Dogma.Views;
using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.Playground.Dogma.ViewModels;

internal class DragonsDogmaMonsterViewModel : ViewModel
{
    public string Name { get; set => SetValue(ref field, value); } = string.Empty;

    public double Health { get; set => SetValue(ref field, value); }

    public double MaxHealth { get; set => SetValue(ref field, value); }

    public int Section { get; set => SetValueThenExecute(ref field, value, CalculateUnits); }

    public int MaxSections { get; set => SetValueThenExecute(ref field, value, CalculateUnits); }

    public bool IsTarget { get; set => SetValue(ref field, value); }

    public ObservableCollection<UnitViewModel> Units { get; } = new();

    private void CalculateUnits()
    {
        if (Units.Count != MaxSections)
        {
            Units.Clear();
            Enumerable.Range(0, MaxSections)
                .ForEach(it => Units.Add(new UnitViewModel(false)));
        }

        for (int i = 0; i < MaxSections; i++)
            Units[i].Value = i < Section;
    }
}