using System.Reflection;

namespace HunterPie.UI.Controls.Settings.ViewModel
{
    public interface ISettingElementType
    {
        public string Name { get; }
        public string Description { get; }
        public object Parent { get; }
        public bool RequiresRestart { get; }
        public PropertyInfo Information { get; }

        public bool Match { get; set; }
    }
}
