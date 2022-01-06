using System.Reflection;
using System.Windows;

namespace HunterPie.UI.Settings.Converter
{
    public interface IVisualConverter
    {
        public FrameworkElement Build(object parent, PropertyInfo childInfo);
    }
}
