using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace HunterPie.UI.Controls.Settings.ViewModel
{
    internal class SettingElementViewModel : ISettingElement
    {
        private readonly string _title;
        private readonly string _description;
        private readonly ImageSource _icon;
        private readonly ObservableCollection<ISettingElementType> _elements = new ObservableCollection<ISettingElementType>();

        public string Title => _title;
        public string Description => _description;
        public ImageSource Icon => _icon;
        public ObservableCollection<ISettingElementType> Elements => _elements;

        public SettingElementViewModel(string title, string description, string icon)
        {
            _title = title;
            _description = description;
            _icon = Application.Current.TryFindResource(icon) as ImageSource;
        }

        public void Add(ISettingElementType element)
        {
            _elements.Add(element);
        }
    }
}
