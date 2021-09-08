using System.Windows;
using System.Windows.Media;

namespace HunterPie.UI.Controls.Settings.ViewModel
{
    public interface ISettingElement
    {
        public string Title { get; }
        public string Description { get; }
        public ImageSource Icon { get; }
        public UIElement Panel { get; }
    }
}
