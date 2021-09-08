using System.Windows;
using System.Windows.Media;

namespace HunterPie.UI.Controls.Settings.ViewModel
{
    internal class SettingElementViewModel : ISettingElement
    {
        private readonly string _title;
        private readonly string _description;
        private readonly ImageSource _icon;
        private readonly UIElement _panel;

        public string Title => _title;
        public string Description => _description;
        public ImageSource Icon => _icon;
        public UIElement Panel => _panel;

        public SettingElementViewModel(string title, string description, string icon, UIElement panel)
        {
            _title = title;
            _description = description;
            _icon = Application.Current.TryFindResource(icon) as ImageSource;
            _panel = panel;
        }
    }
}
