using System.Collections.ObjectModel;

namespace HunterPie.UI.Settings.ViewModels.Internal;

#nullable enable
internal class EnumPropertyViewModel(object selectedElement, ObservableCollection<object> values) : ConfigurationPropertyViewModel
{
    public object SelectedElement { get; set => SetValue(ref field, value); } = selectedElement;
    public ObservableCollection<object> Values { get; } = values;
}