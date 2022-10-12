using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace HunterPie.UI.Controls.Settings.ViewModel;

internal class SettingElementViewModel : ISettingElement
{
    public string Title { get; }
    public string Description { get; }
    public ImageSource Icon { get; }
    public ObservableCollection<ISettingElementType> Elements { get; } = new();

    public SettingElementViewModel(string title, string description, string icon)
    {
        Title = title;
        Description = description;
        Icon = Application.Current.TryFindResource(icon) as ImageSource;
    }

    public void Add(ISettingElementType element) => Elements.Add(element);
}
