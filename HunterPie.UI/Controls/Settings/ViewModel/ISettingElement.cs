using System.Collections.ObjectModel;
using System.Windows.Media;

namespace HunterPie.UI.Controls.Settings.ViewModel
{
    public interface ISettingElement
    {
        public string Title { get; }
        public string Description { get; }
        public ImageSource Icon { get; }
        public ObservableCollection<ISettingElementType> Elements { get; }

        public void Add(ISettingElementType element);
    }
}
