using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace HunterPie.UI.Controls.Settings.ViewModel
{
    internal class MockSettingHostViewModel
    {
        public class MockSettingElement : ISettingElement
        {
            public string Title => "Mock";
            public string Description => "Mock";
            public ImageSource Icon => Application.Current.FindResource("ICON_BUG") as ImageSource;
            public string Mock = "Mock";

            public ObservableCollection<ISettingElementType> Elements { get; } = new();

            public void Add(ISettingElementType element) { }

            public MockSettingElement()
            {
                for (int i = 0; i < 15; i++)
                    Elements.Add(new SettingElementType("Mock", "Mock", this, GetType().GetProperty(nameof(Mock))));
            }
        }

        public ObservableCollection<ISettingElement> Elements { get; } = new()
        {
            new MockSettingElement(),
            new MockSettingElement(),
        };
    }
}
