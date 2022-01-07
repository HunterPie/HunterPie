using HunterPie.Core.Settings.Types;
using HunterPie.UI.Settings.Converter;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using RangeUI = HunterPie.UI.Controls.Sliders.Range;

namespace HunterPie.UI.Settings.Internal
{
    internal class RangeVisualConverter : IVisualConverter
    {
        public FrameworkElement Build(object parent, PropertyInfo childInfo)
        {
            Range range = (Range)childInfo.GetValue(parent);
            var currentBinding = VisualConverterHelper.CreateBinding(range, nameof(Range.Current));
            var maxBinding = VisualConverterHelper.CreateBinding(range, nameof(Range.Max));
            var minBinding = VisualConverterHelper.CreateBinding(range, nameof(Range.Min));
            var stepBinding = VisualConverterHelper.CreateBinding(range, nameof(Range.Step));

            RangeUI slider = new();

            BindingOperations.SetBinding(slider, RangeUI.ValueProperty, currentBinding);
            BindingOperations.SetBinding(slider, RangeUI.MaximumProperty, maxBinding);
            BindingOperations.SetBinding(slider, RangeUI.MinimumProperty, minBinding);
            BindingOperations.SetBinding(slider, RangeUI.ChangeProperty, stepBinding);

            return slider;
        }
    }
}
