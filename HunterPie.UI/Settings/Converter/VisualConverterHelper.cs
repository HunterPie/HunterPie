using HunterPie.Core.Architecture;
using HunterPie.Core.Settings.Types;
using System.Windows.Data;

namespace HunterPie.UI.Settings.Converter
{
    public static class VisualConverterHelper
    {
        public static Binding CreateBinding(object parent, string path = nameof(Observable<bool>.Value))
        {
            return new Binding(path)
            {
                Mode = BindingMode.TwoWay,
                Source = parent
            };
        }
    }
}
