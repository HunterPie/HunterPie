using System.Reflection;
using System.Windows;

namespace HunterPie.UI.Settings.Converter
{
    public interface IVisualConverter
    {
        public UIElement Build(object parent, PropertyInfo childInfo);
    }
}
