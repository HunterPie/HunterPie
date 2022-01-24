using HunterPie.Core.Architecture;
using System.Reflection;

namespace HunterPie.UI.Controls.Settings.ViewModel
{
    internal class SettingElementType : Bindable, ISettingElementType
    {
        private bool _match = true;

        public object Parent { get; private set; }
        public PropertyInfo Information { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool RequiresRestart { get; private set; }

        public bool Match
        {
            get => _match;
            set { SetValue(ref _match, value); }
        }

        public SettingElementType(string name, string description, object parent, PropertyInfo info, bool restart)
        {
            Name = name;
            Description = description;
            Parent = parent;
            Information = info;
            RequiresRestart = restart;
        }
    }
}
